using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebMunicipio.Models;
using WebMunicipio.Services;

namespace WebMunicipio.Controllers
{
    public class ReportesController : Controller
    {
        private readonly ReporteService _reporteService;

        public ReportesController(ReporteService reporteService)
        {
            _reporteService = reporteService;
        }

        public async Task<IActionResult> Index()
        {
            var reportes = await _reporteService.ObtenerReporte();

            return View(reportes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Reporte reporte)
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

                return View(reporte);
            }

            bool creado = await _reporteService.CrearReporte(reporte);

            if (!creado)
            {
                ViewBag.Error = "No se pudo crear el reporte.";
                return View(reporte);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var reporte = await _reporteService.ObtenerReporte(id);

            return View(reporte);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Reporte reporte)
        {
            if (!ModelState.IsValid)
            {
                return View(reporte);
            }

            bool actualizado = await _reporteService.EditarReporte(reporte);

            if (!actualizado)
            {
                ViewBag.Error = "No se pudo actualizar el reporte.";
                return View(reporte);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var reporte = await _reporteService.ObtenerReporte(id);

            return View(reporte);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Reporte reporte)
        {
            await _reporteService.EliminarReporte(reporte.IdReporte);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var reporte = await _reporteService.ObtenerReporte(id);

            if (reporte == null)
            {
                return NotFound();
            }

            return View(reporte);
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarEstado(int id, string estado)
        {
            bool actualizado = await _reporteService.ActualizarEstado(id, estado);

            return RedirectToAction(nameof(Index));
        }

    }

}
