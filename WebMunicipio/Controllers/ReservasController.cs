using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebMunicipio.Models;
using WebMunicipio.Services;

namespace WebMunicipio.Controllers
{
    public class ReservasController : Controller
    {
        private readonly ReservaService _reservaService;

        public ReservasController(ReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        public async Task<IActionResult> Index()
        {
            var reservas = await _reservaService.ObtenerReservas();

            return View(reservas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Reserva reserva)
        {
            await _reservaService.CrearReserva(reserva);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var reserva = await _reservaService.ObtenerReserva(id);

            return View(reserva);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Reserva reserva)
        {
            await _reservaService.EditarReserva(reserva);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var reserva = await _reservaService.ObtenerReserva(id);

            return View(reserva);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Reserva reserva)
        {
            await _reservaService.EliminarReserva(reserva.IdReserva);

            return RedirectToAction(nameof(Index));
        }
    }
}
