using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BARmonica.Web.Models;
using BARmonica.Web.Services;
using Microsoft.AspNetCore.Authorization;

namespace BARmonica.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IProducerService _producerService;

        // Injectam toate cele 3 servicii necesare
        public ProductsController(
            IProductService productService,
            ICategoryService categoryService,
            IProducerService producerService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _producerService = producerService;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        // GET: Products/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _productService.GetProductByIdAsync(id.Value);
            if (product == null) return NotFound();

            return View(product);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            var producers = await _producerService.GetAllProducersAsync();

            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name");
            ViewData["ProducerId"] = new SelectList(producers, "Id", "Name");

            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Specifications,Price,StockQuantity,DeliveryMethod,ImageUrl,CategoryId,ProducerId")] Product product)
        {
            if (ModelState.IsValid)
            {
                await _productService.AddProductAsync(product);
                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryService.GetAllCategoriesAsync();
            var producers = await _producerService.GetAllProducersAsync();
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", product.CategoryId);
            ViewData["ProducerId"] = new SelectList(producers, "Id", "Name", product.ProducerId);

            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _productService.GetProductByIdAsync(id.Value);
            if (product == null) return NotFound();

            var categories = await _categoryService.GetAllCategoriesAsync();
            var producers = await _producerService.GetAllProducersAsync();
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", product.CategoryId);
            ViewData["ProducerId"] = new SelectList(producers, "Id", "Name", product.ProducerId);

            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Specifications,Price,StockQuantity,DeliveryMethod,ImageUrl,CategoryId,ProducerId")] Product product)
        {
            if (id != product.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _productService.UpdateProductAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _productService.GetProductByIdAsync(product.Id) == null)
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryService.GetAllCategoriesAsync();
            var producers = await _producerService.GetAllProducersAsync();
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", product.CategoryId);
            ViewData["ProducerId"] = new SelectList(producers, "Id", "Name", product.ProducerId);

            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _productService.GetProductByIdAsync(id.Value);
            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}