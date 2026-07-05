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
            var lista = await _alertaService.ObtenerAlertas();

            lista = lista
                .OrderBy(a => a.EstadoAlerta != "Activa") 
                .ThenByDescending(a => a.Fecha)          
                .ToList();

            return View(lista);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Alerta alerta)
        {
            if (!ModelState.IsValid)
                return View(alerta);

            bool creado = await _alertaService.CrearAlerta(alerta);

            if (!creado)
            {
                ViewBag.Error = "No fue posible registrar la alerta.";

                return View(alerta);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var alerta = await _alertaService.ObtenerAlerta(id);

            return View(alerta);
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

        [HttpPost]
        public async Task<IActionResult> ActualizarEstado(int id, string estado)
        {
            await _alertaService.ActualizarEstado(id, estado);

            return RedirectToAction(nameof(Index));
        }
    }
}