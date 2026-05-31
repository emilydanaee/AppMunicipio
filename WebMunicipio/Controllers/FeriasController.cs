using Microsoft.AspNetCore.Mvc;
using WebMunicipio.Models;
using WebMunicipio.Services;

namespace WebMunicipio.Controllers
{
    public class FeriasController : Controller
    {
        private readonly FeriaService _feriaService;

        public FeriasController(FeriaService feriaService)
        {
            _feriaService = feriaService;
        }

        public async Task<IActionResult> Index()
        {
            var ferias = await _feriaService.ObtenerFerias();

            return View(ferias);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Feria feria)
        {
            await _feriaService.CrearFeria(feria);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var feria = await _feriaService.ObtenerFeria(id);

            return View(feria);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Feria feria)
        {
            await _feriaService.EditarFeria(feria);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var feria = await _feriaService.ObtenerFeria(id);

            return View(feria);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _feriaService.EliminarFeria(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
