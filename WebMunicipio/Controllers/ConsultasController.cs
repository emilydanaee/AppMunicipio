using Microsoft.AspNetCore.Mvc;
using WebMunicipio.Models;
using WebMunicipio.Services;

namespace WebMunicipio.Controllers
{
    public class ConsultasController : Controller
    {
        private readonly ConsultaService _consultaService;

        public ConsultasController(ConsultaService consultaService)
        {
            _consultaService = consultaService;
        }

        public async Task<IActionResult> Index()
        {
            var consultas = await _consultaService.ObtenerConsulta();

            return View(consultas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Consulta consulta)
        {
            await _consultaService.CrearConsulta(consulta);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var consulta = await _consultaService.ObtenerConsulta(id);

            return View(consulta);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Consulta consulta)
        {
            await _consultaService.EditarConsulta(consulta);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var consulta = await _consultaService.ObtenerConsulta(id);

            return View(consulta);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Consulta consulta)
        {
            await _consultaService.EliminarConsulta(consulta.IdConsulta);

            return RedirectToAction(nameof(Index));
        }
    }
}
