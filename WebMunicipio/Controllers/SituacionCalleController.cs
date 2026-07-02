using Microsoft.AspNetCore.Mvc;
using WebMunicipio.Models;
using WebMunicipio.Services;

namespace WebMunicipio.Controllers
{
    public class SituacionCalleController : Controller
    {
        private readonly SituacionCalleService _situacionCalleService;

        public SituacionCalleController(SituacionCalleService situacionCalleService)
        {
            _situacionCalleService = situacionCalleService;
        }

        public async Task<IActionResult> Index()
        {
            var situacionCalle = await _situacionCalleService.ObtenerSituacionCalle();

            return View(situacionCalle);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SituacionCalle situacionCalle)
        {
            await _situacionCalleService.CrearSituacionCalle(situacionCalle);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var situacionCalle = await _situacionCalleService.ObtenerSituacionCalle(id);

            return View(situacionCalle);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SituacionCalle situacionCalle)
        {
            await _situacionCalleService.EditarSituacionCalle(situacionCalle);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var situacionCalle = await _situacionCalleService.ObtenerSituacionCalle(id);

            return View(situacionCalle);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SituacionCalle situacionCalle)
        {
            await _situacionCalleService.EliminarSituacionCalle(situacionCalle.IdSituacionCalle);

            return RedirectToAction(nameof(Index));
        }
    }
}
