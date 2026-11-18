using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BARmonica.Web.Models;
using BARmonica.Web.Data;
using BARmonica.Web.Helpers;

namespace BARmonica.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static readonly string[] AllowedStatuses =
        {
            "În procesare",
            "Expediată",
            "Livrată",
            "Anulată"
        };

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Vizualizare Coș (accesibil și neconectat)
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            return View(cart);
        }

        // 2. Adăugare produs în coș (cu verificare de stoc)
        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            if (product.StockQuantity <= 0)
            {
                TempData["CartMessage"] = $"Produsul \"{product.Name}\" are stocul epuizat.";
                return RedirectToAction("Index");
            }

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var existingItem = cart.FirstOrDefault(x => x.ProductId == id);

            if (existingItem != null)
            {
                if (existingItem.Quantity + 1 > product.StockQuantity)
                {
                    existingItem.Quantity = product.StockQuantity;
                    TempData["CartMessage"] = $"Stoc maxim atins pentru \"{product.Name}\" ({product.StockQuantity} buc. disponibile).";
                }
                else
                {
                    existingItem.Quantity++;
                }
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = 1
                });
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("Index");
        }

        // 3. Modificare Cantitate (cu limită de stoc și ștergere la <= 0)
        [HttpPost]
        public IActionResult Update(int id, int quantity)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
            if (cart != null)
            {
                var item = cart.FirstOrDefault(x => x.ProductId == id);
                if (item != null)
                {
                    if (quantity <= 0)
                    {
                        cart.Remove(item);
                    }
                    else
                    {
                        var product = _context.Products.FirstOrDefault(p => p.Id == id);
                        if (product == null || product.StockQuantity <= 0)
                        {
                            cart.Remove(item);
                            TempData["CartMessage"] = $"Produsul \"{item.Name}\" nu mai este disponibil în stoc și a fost eliminat din coș.";
                        }
                        else if (quantity > product.StockQuantity)
                        {
                            item.Quantity = product.StockQuantity;
                            TempData["CartMessage"] = $"Nu poți comanda {quantity} buc. din \"{product.Name}\". Stoc disponibil: {product.StockQuantity} buc.";
                        }
                        else
                        {
                            item.Quantity = quantity;
                        }
                    }

                    HttpContext.Session.SetObjectAsJson("Cart", cart);
                }
            }
            return RedirectToAction("Index");
        }

        // 4. Ștergere Produs
        public IActionResult Remove(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
            if (cart != null)
            {
                cart.RemoveAll(x => x.ProductId == id);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }

        // 5. Pagina Checkout (blochează accesul dacă nu ești logat sau dacă ai coșul gol)
        [Authorize]
        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            if (!cart.Any())
            {
                TempData["CartMessage"] = "Coșul tău este gol. Adaugă cel puțin un produs înainte de a finaliza comanda.";
                return RedirectToAction("Index");
            }

            return View();
        }

        // 6. Finalizare Comandă (re-verifică stocul înainte de salvare)
        [Authorize]
        [HttpPost]
        public IActionResult PlaceOrder(string? shippingAddress, string? paymentMethod)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            if (!cart.Any())
            {
                TempData["CartMessage"] = "Coșul tău este gol. Nu se poate plasa o comandă fără produse.";
                return RedirectToAction("Index");
            }

            // Re-verificare finală a stocului real din baza de date
            var stockAdjusted = false;
            foreach (var item in cart.ToList())
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product == null || product.StockQuantity <= 0)
                {
                    cart.Remove(item);
                    stockAdjusted = true;
                }
                else if (item.Quantity > product.StockQuantity)
                {
                    item.Quantity = product.StockQuantity;
                    stockAdjusted = true;
                }
            }

            if (stockAdjusted)
            {
                HttpContext.Session.SetObjectAsJson("Cart", cart);
                TempData["CartMessage"] = "Stocul unora dintre produse s-a modificat. Am actualizat cantitățile din coș — te rugăm să verifici înainte de a plasa comanda.";
                return RedirectToAction("Index");
            }

            var productsSummary = string.Join(", ", cart.Select(c => $"{c.Name} (x{c.Quantity})"));
            var currentUser = User.Identity?.Name ?? "Client BARmonica";

            var addressText = !string.IsNullOrWhiteSpace(shippingAddress) ? shippingAddress : "Adresă standard";
            var paymentText = !string.IsNullOrWhiteSpace(paymentMethod) ? $" | Plată: {paymentMethod}" : "";

            var order = new Order
            {
                OrderDate = DateTime.Now,
                CustomerName = currentUser,
                ShippingAddress = $"{addressText}{paymentText} | Produse: {productsSummary}",
                Status = "În procesare",
                TotalAmount = cart.Sum(x => x.Price * x.Quantity)
            };

            _context.Orders.Add(order);

            foreach (var item in cart)
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product != null)
                {
                    product.StockQuantity = Math.Max(0, product.StockQuantity - item.Quantity);
                }
            }

            _context.SaveChanges();
            HttpContext.Session.Remove("Cart");
            return View("OrderSuccess");
        }

        // 7. Istoric Comenzi
        [Authorize]
        public IActionResult OrderHistory()
        {
            var currentUser = User.Identity?.Name;
            var query = _context.Orders.AsQueryable();

            if (!User.IsInRole("Admin") && !string.IsNullOrEmpty(currentUser))
            {
                query = query.Where(o => o.CustomerName == currentUser);
            }

            var orders = query.OrderByDescending(o => o.OrderDate).ToList();
            return View(orders);
        }

        // 8. Schimbare Status Comandă (doar Admin)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrderStatus(int id, string status)
        {
            if (!AllowedStatuses.Contains(status))
            {
                return RedirectToAction("OrderHistory");
            }

            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order != null)
            {
                order.Status = status;
                _context.SaveChanges();
                TempData["OrderMessage"] = $"Statusul comenzii #{order.Id} a fost schimbat în „{status}”.";
            }

            return RedirectToAction("OrderHistory");
        }
    }
}