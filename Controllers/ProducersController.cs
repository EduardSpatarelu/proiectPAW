using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BARmonica.Web.Models;
using BARmonica.Web.Services;
using Microsoft.AspNetCore.Authorization;

namespace BARmonica.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProducersController : Controller
    {
        // Aici Chelnerul primeste comenzi si le da Bucatarului (ProducerService)
        private readonly IProducerService _producerService;

        public ProducersController(IProducerService producerService)
        {
            _producerService = producerService;
        }

        // GET: Producers
        public async Task<IActionResult> Index()
        {
            var producers = await _producerService.GetAllProducersAsync();
            return View(producers);
        }

        // GET: Producers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var producer = await _producerService.GetProducerByIdAsync(id.Value);
            if (producer == null) return NotFound();

            return View(producer);
        }

        // GET: Producers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Producers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CountryOfOrigin")] Producer producer)
        {
            if (ModelState.IsValid)
            {
                await _producerService.AddProducerAsync(producer);
                return RedirectToAction(nameof(Index));
            }
            return View(producer);
        }

        // GET: Producers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var producer = await _producerService.GetProducerByIdAsync(id.Value);
            if (producer == null) return NotFound();

            return View(producer);
        }

        // POST: Producers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CountryOfOrigin")] Producer producer)
        {
            if (id != producer.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _producerService.UpdateProducerAsync(producer);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _producerService.GetProducerByIdAsync(producer.Id) == null)
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(producer);
        }

        // GET: Producers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var producer = await _producerService.GetProducerByIdAsync(id.Value);
            if (producer == null) return NotFound();

            return View(producer);
        }

        // POST: Producers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _producerService.DeleteProducerAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}