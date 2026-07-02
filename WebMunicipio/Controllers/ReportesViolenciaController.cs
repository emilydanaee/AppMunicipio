using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebMunicipio.Models;
using WebMunicipio.Services;

namespace WebMunicipio.Controllers
{
    public class ReportesViolenciaController : Controller
    {
        private readonly ReporteViolenciaService _reporteViolenciaService;

        public ReportesViolenciaController(ReporteViolenciaService reporteViolenciaService)
        {
            _reporteViolenciaService = reporteViolenciaService;
        }

        public async Task<IActionResult> Index()
        {
            var reportesViolencia = await _reporteViolenciaService.ObtenerReportesViolencia();

            return View(reportesViolencia);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReporteViolencia reporteViolencia)
        {
            await _reporteViolenciaService.CrearReporteViolencia(reporteViolencia);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var reporteViolencia = await _reporteViolenciaService.ObtenerReporteViolencia(id);

            return View(reporteViolencia);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ReporteViolencia reporteViolencia)
        {
            await _reporteViolenciaService.EditarReporteViolencia(reporteViolencia);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var reporteViolencia = await _reporteViolenciaService.ObtenerReporteViolencia(id);

            return View(reporteViolencia);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ReporteViolencia reporteViolencia)
        {
            await _reporteViolenciaService.EliminarReporteViolencia(reporteViolencia.IdReporteViolencia);

            return RedirectToAction(nameof(Index));
        }
    }
}
