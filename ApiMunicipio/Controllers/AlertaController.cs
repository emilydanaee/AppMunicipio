using ApiMunicipio.Data;
using ApiMunicipio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMunicipio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertaController : ControllerBase
    {
        private readonly MunicipioContext _context;

        public AlertaController(MunicipioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alerta>>> GetAlerta()
        {
            return Ok(await _context.Alertas.OrderByDescending(x => x.Fecha).ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Alerta>> GetAlertaByID(int id)
        {
            var alerta = await _context.Alertas.FindAsync(id);

            if (alerta == null)
                return NotFound();

            return Ok(alerta);
        }

        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<Alerta>>> Buscar(string texto)
        {
            var alertas = await _context.Alertas
                .Where(a =>
                    a.DescripcionAlerta.Contains(texto) ||
                    a.TipoAlerta.Contains(texto))
                .OrderByDescending(a => a.Fecha)
                .ToListAsync();

            return Ok(alertas);
        }

        [HttpGet("estado")]
        public async Task<ActionResult<IEnumerable<Alerta>>> Estado(string estado)
        {
            var alertas = await _context.Alertas
                .Where(a => a.EstadoAlerta == estado)
                .OrderByDescending(a => a.Fecha)
                .ToListAsync();

            return Ok(alertas);
        }

        [HttpGet("tipo")]
        public async Task<ActionResult<IEnumerable<Alerta>>> Tipo(string tipo)
        {
            var alertas = await _context.Alertas
                .Where(a => a.TipoAlerta == tipo)
                .OrderByDescending(a => a.Fecha)
                .ToListAsync();

            return Ok(alertas);
        }

        [HttpGet("sector")]
        public async Task<ActionResult<IEnumerable<Alerta>>> Sector(string sector)
        {
            var alertas = await _context.Alertas
                .Where(a => a.Sector == sector)
                .OrderByDescending(a => a.Fecha)
                .ToListAsync();

            return Ok(alertas);
        }

        [HttpPost]
        public async Task<ActionResult<Alerta>> AddAlerta(Alerta newAlerta)
        {
            if (newAlerta == null) return BadRequest();

            _context.Alertas.Add(newAlerta);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAlertaByID),
                new { id = newAlerta.IdAlerta }, newAlerta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAlerta(int id, Alerta alerta)
        {
            if (id != alerta.IdAlerta)
                return BadRequest();

            _context.Entry(alerta).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlerta(int id)
        {
            var alerta = await _context.Alertas.FindAsync(id);

            if (alerta == null)
                return NotFound();

            _context.Alertas.Remove(alerta);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
