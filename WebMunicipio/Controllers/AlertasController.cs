using Microsoft.AspNetCore.Mvc;
using WebMunicipio.Models;
using WebMunicipio.Services;

namespace WebMunicipio.Controllers
{
    public class AlertasController : Controller
    {
        private readonly AlertaService _alertaService;

        public AlertasController(AlertaService alertaService)
        {
            _alertaService = alertaService;
        }

        public async Task<IActionResult> Index()
        {
            var alertas = await _alertaService.ObtenerAlertas();

            return View(alertas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Alerta alerta)
        {
            await _alertaService.CrearAlerta(alerta);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var alerta = await _alertaService.ObtenerAlerta(id);

            return View(alerta);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Alerta alerta)
        {
            await _alertaService.EditarAlerta(alerta);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var alerta = await _alertaService.ObtenerAlerta(id);

            return View(alerta);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Alerta alerta)
        {
            await _alertaService.EliminarAlerta(alerta.IdAlerta);

            return RedirectToAction(nameof(Index));
        }
    }
}
