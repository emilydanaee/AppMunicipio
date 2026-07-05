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
            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        Console.WriteLine($"{item.Key} -> {error.ErrorMessage}");
                    }
                }

                return View(campania);
            }

            bool creado = await _campaniaService.CrearCampania(campania);

            if (!creado)
            {
                ViewBag.Error = "No se pudo crear la campaña.";
                return View(campania);
            }

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
            if (!ModelState.IsValid)
            {
                return View(campania);
            }

            bool actualizado = await _campaniaService.EditarCampania(campania);

            if (!actualizado)
            {
                ViewBag.Error = "No se pudo actualizar la campaña.";
                return View(campania);
            }

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


        public async Task<IActionResult> Details(int id)
        {
            var campania = await _campaniaService.ObtenerCampania(id);

            if (campania == null)
            {
                return NotFound();
            }

            return View(campania);
        }
    }
}
