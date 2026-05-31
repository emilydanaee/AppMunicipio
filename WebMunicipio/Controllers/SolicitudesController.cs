using Microsoft.AspNetCore.Mvc;
using WebMunicipio.Models;
using WebMunicipio.Services;

namespace WebMunicipio.Controllers
{
    public class SolicitudesController : Controller
    {
        private readonly SolicitudService _solicitudService;

        public SolicitudesController(SolicitudService solicitudService)
        {
            _solicitudService = solicitudService;
        }

        public async Task<IActionResult> Index()
        {
            var solicitudes = await _solicitudService.ObtenerSolicitudes();

            return View(solicitudes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Solicitud solicitud)
        {
            await _solicitudService.CrearSolicitud(solicitud);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var solicitud = await _solicitudService.ObtenerSolicitud(id);

            return View(solicitud);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Solicitud solicitud)
        {
            await _solicitudService.EditarSolicitud(solicitud);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var solicitud = await _solicitudService.ObtenerSolicitud(id);

            return View(solicitud);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Solicitud solicitud)
        {
            await _solicitudService.EliminarSolicitud(solicitud.IdSolicitud);

            return RedirectToAction(nameof(Index));
        }
    }
}
