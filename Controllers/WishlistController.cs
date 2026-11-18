using Microsoft.AspNetCore.Mvc;
using BARmonica.Web.Models;
using BARmonica.Web.Services;
using System.Text.Json;

namespace BARmonica.Web.Controllers
{
    public class WishlistController : Controller
    {
        private readonly IProductService _productService;
        private const string WishlistSessionKey = "Wishlist";

        public WishlistController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: /Wishlist
        public async Task<IActionResult> Index()
        {
            var productIds = GetWishlistIds();
            var products = new List<Product>();

            foreach (var id in productIds)
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product != null)
                {
                    products.Add(product);
                }
            }

            return View(products);
        }

        // POST: /Wishlist/AddToWishlist
        [HttpPost]
        public IActionResult AddToWishlist(int id)
        {
            var productIds = GetWishlistIds();
            if (!productIds.Contains(id))
            {
                productIds.Add(id);
                SaveWishlistIds(productIds);
            }

            return RedirectToAction("Index");
        }

        // POST: /Wishlist/RemoveFromWishlist
        [HttpPost]
        public IActionResult RemoveFromWishlist(int id)
        {
            var productIds = GetWishlistIds();
            if (productIds.Contains(id))
            {
                productIds.Remove(id);
                SaveWishlistIds(productIds);
            }

            return RedirectToAction("Index");
        }

        private List<int> GetWishlistIds()
        {
            var json = HttpContext.Session.GetString(WishlistSessionKey);
            return string.IsNullOrEmpty(json)
                ? new List<int>()
                : JsonSerializer.Deserialize<List<int>>(json) ?? new List<int>();
        }

        private void SaveWishlistIds(List<int> ids)
        {
            var json = JsonSerializer.Serialize(ids);
            HttpContext.Session.SetString(WishlistSessionKey, json);
        }
    }
}