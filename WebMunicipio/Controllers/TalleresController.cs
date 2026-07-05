using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebMunicipio.Models;
using WebMunicipio.Services;

namespace WebMunicipio.Controllers
{
    public class TalleresController : Controller
    {
        private readonly TallerService _tallerService;

        public TalleresController(TallerService tallerService)
        {
            _tallerService = tallerService;
        }

        public async Task<IActionResult> Index()
        {
            var talleres = await _tallerService.ObtenerTalleres();

            return View(talleres);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Taller taller)
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

                return View(taller);
            }

            bool creado = await _tallerService.CrearTaller(taller);

            if (!creado)
            {
                ViewBag.Error = "No se pudo crear el taller.";
                return View(taller);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var taller = await _tallerService.ObtenerTaller(id);

            return View(taller);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Taller taller)
        {
            if (!ModelState.IsValid)
            {
                return View(taller);
            }

            bool actualizado = await _tallerService.EditarTaller(taller);

            if (!actualizado)
            {
                ViewBag.Error = "No se pudo actualizar el taller.";
                return View(taller);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var taller = await _tallerService.ObtenerTaller(id);

            return View(taller);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Taller taller)
        {
            await _tallerService.EliminarTaller(taller.IdTaller);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var taller = await _tallerService.ObtenerTaller(id);

            if (taller == null)
            {
                return NotFound();
            }

            return View(taller);
        }

    }
}
