using BARmonica.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using BARmonica.Web.Models;

namespace BARmonica.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Injectăm baza de date pentru a putea extrage produsele
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Aici este "inima" magazinului: primim comenzi de sortare, categorie si cautare
        public async Task<IActionResult> Index(string category, string sortOrder, string searchString)
        {
            // Salvăm selecțiile în ViewData pentru a le menține active în butoane
            ViewData["CurrentCategory"] = category;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentSearch"] = searchString;

            // 1. Extragem toate produsele și includem datele despre Categorie
            var products = _context.Products.Include(p => p.Category).AsQueryable();

            // 2. FILTRARE: Căutare după nume (Bara de căutare)
            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString));
            }

            // 3. FILTRARE: După Categorie (Meniul Slide)
            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category.Name == category);
            }

            // 4. SORTARE: Crescător, Descrescător, A-Z
            products = sortOrder switch
            {
                "price_asc" => products.OrderBy(p => p.Price),
                "price_desc" => products.OrderByDescending(p => p.Price),
                "name_desc" => products.OrderByDescending(p => p.Name),
                _ => products.OrderBy(p => p.Name), // Implicit sortează A-Z
            };

            // 5. Trimitem lista finală către pagina vizuală
            return View(await products.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}