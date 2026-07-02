using Microsoft.AspNetCore.Mvc;
using WebMunicipio.Models;
using WebMunicipio.Services;

namespace WebMunicipio.Controllers
{
    public class CampaniasController : Controller
    {
        private readonly CampaniaService _campaniaService;

        public CampaniasController(CampaniaService campaniaService)
        {
            _campaniaService = campaniaService;
        }

        public async Task<IActionResult> Index()
        {
            var campanias = await _campaniaService.ObtenerCampania();

            return View(campanias);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Campania campania)
        {
            await _campaniaService.CrearCampania(campania);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var campania = await _campaniaService.ObtenerCampania(id);

            return View(campania);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Campania campania)
        {
            await _campaniaService.EditarCampania(campania);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var campania = await _campaniaService.ObtenerCampania(id);

            return View(campania);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Campania campania)
        {
            await _campaniaService.EliminarCampania(campania.IdCampania);

            return RedirectToAction(nameof(Index));
        }
    }
}
