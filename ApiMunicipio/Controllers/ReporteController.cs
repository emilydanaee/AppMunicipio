using ApiMunicipio.Data;
using ApiMunicipio.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMunicipio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReporteController : ControllerBase
    {
        private readonly MunicipioContext _context;

        public ReporteController(MunicipioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Reporte>>> GetReportes()
        {
            return Ok(await _context.Reportes.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reporte>> GetReporteByID(int id)
        {
            var reporte = await _context.Reportes.FindAsync(id);

            if (reporte == null)
                return NotFound();

            return Ok(reporte);
        }

        [HttpPost]
        public async Task<ActionResult<Reporte>> AddReporte(Reporte newReporte)
        {
            _context.Reportes.Add(newReporte);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReporteByID),
                new { id = newReporte.IdReporte }, newReporte);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReporte(int id, Reporte reporte)
        {
            if (id != reporte.IdReporte)
                return BadRequest();

            _context.Entry(reporte).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReporte(int id)
        {
            var reporte = await _context.Reportes.FindAsync(id);

            if (reporte == null)
                return NotFound();

            _context.Reportes.Remove(reporte);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
