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
            await _reporteService.CrearReporte(reporte);

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
            await _reporteService.EditarReporte(reporte);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var reporte = await _reporteService.ObtenerReporte(id);

            return View(reporte);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _reporteService.EliminarReporte(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
