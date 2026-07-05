using Microsoft.AspNetCore.Mvc;
using WebMunicipio.Models;
using WebMunicipio.Services;

namespace WebMunicipio.Controllers
{
    public class SituacionCalleController : Controller
    {
        private readonly SituacionCalleService _service;

        public SituacionCalleController(
            SituacionCalleService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var lista = await _service.ObtenerSituaciones();

            return View(lista);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            SituacionCalle situacion)
        {
            if (!ModelState.IsValid)
                return View(situacion);

            bool creado = await _service.CrearSituacion(situacion);

            if (!creado)
            {
                ViewBag.Error = "No fue posible registrar el reporte.";

                return View(situacion);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var situacion = await _service.ObtenerSituacion(id);

            if (situacion == null)
                return NotFound();

            return View(situacion);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var situacion = await _service.ObtenerSituacion(id);

            if (situacion == null)
                return NotFound();

            return View(situacion);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(
            SituacionCalle situacion)
        {
            if (!ModelState.IsValid)
                return View(situacion);

            await _service.EditarSituacion(situacion);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var situacion = await _service.ObtenerSituacion(id);

            if (situacion == null)
                return NotFound();

            return View(situacion);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(
            SituacionCalle situacion)
        {
            await _service.EliminarSituacion(
                situacion.IdSituacionCalle);

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> ActualizarEstado(int id, string estado)
        {
            await _service.ActualizarEstado(id, estado);

            return RedirectToAction(nameof(Index));
        }
    }
}