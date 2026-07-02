using ApiMunicipio.Data;
using ApiMunicipio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMunicipio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private readonly MunicipioContext _context;

        public ReservaController(MunicipioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetReservas()
        {
            return Ok(await _context.Reservas.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reserva>> GetReservaByID(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva == null)
                return NotFound();

            return Ok(reserva);
        }

        [HttpPost]
        public async Task<ActionResult<Reserva>> AddReserva(Reserva newReserva)
        {
            if (newReserva == null)
                return BadRequest();
            _context.Reservas.Add(newReserva);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReservaByID),
                new { id = newReserva.IdReserva }, newReserva);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReserva(int id, Reserva reserva)
        {
            if (id != reserva.IdReserva)
                return BadRequest();

            _context.Entry(reserva).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReserva(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva == null)
                return NotFound();

            _context.Reservas.Remove(reserva);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
