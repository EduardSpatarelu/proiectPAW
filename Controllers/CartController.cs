using Microsoft.AspNetCore.Mvc;
using BARmonica.Web.Models;
using BARmonica.Web.Data;
using BARmonica.Web.Helpers;

namespace BARmonica.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Vizualizare Coș
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            return View(cart);
        }

        // 2. Adăugare produs (cu preț luat direct din DB)
        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            // Căutăm produsul în baza de date ca să îi luăm prețul corect
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var existingItem = cart.FirstOrDefault(x => x.ProductId == id);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price, // Prețul real
                    Quantity = 1
                });
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("Index");
        }

        // 3. Modificare Cantitate
        [HttpPost]
        public IActionResult Update(int id, int quantity)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
            if (cart != null)
            {
                var item = cart.FirstOrDefault(x => x.ProductId == id);
                if (item != null && quantity > 0)
                {
                    item.Quantity = quantity;
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

        // 5. Pagina Checkout
        public IActionResult Checkout()
        {
            return View();
        }

        // 6. Finalizare Comandă
        [HttpPost]
        public IActionResult PlaceOrder()
        {
            // Aici ar trebui salvată comanda în baza de date (dacă vrei să duci proiectul la nivelul următor)
            // Pentru acum, golim doar sesiunea
            HttpContext.Session.Remove("Cart");
            return View("OrderSuccess");
        }
    }
}