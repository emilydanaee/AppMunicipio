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
            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        Console.WriteLine($"{item.Key} -> {error.ErrorMessage}");
                    }
                }

                return View(feria);
            }

            bool creado = await _feriaService.CrearFeria(feria);

            if (!creado)
            {
                ViewBag.Error = "No se pudo crear la feria.";
                return View(feria);
            }

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
            if (!ModelState.IsValid)
            {
                return View(feria);
            }

            bool actualizado = await _feriaService.EditarFeria(feria);

            if (!actualizado)
            {
                ViewBag.Error = "No se pudo actualizar la feria.";
                return View(feria);
            }

            return RedirectToAction(nameof(Index));
        }
        

        public async Task<IActionResult> Delete(int id)
        {
            var feria = await _feriaService.ObtenerFeria(id);

            return View(feria);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Feria feria)
        {
            await _feriaService.EliminarFeria(feria.IdFeria);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var feria = await _feriaService.ObtenerFeria(id);

            if (feria == null)
            {
                return NotFound();
            }

            return View(feria);
        }
    }
}
