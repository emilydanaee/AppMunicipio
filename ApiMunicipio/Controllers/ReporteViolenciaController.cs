using ApiMunicipio.Models;
using ApiMunicipio.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppMunicipio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReporteViolenciaController : ControllerBase
    {
        private readonly MunicipioContext _context;

        public ReporteViolenciaController(MunicipioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReporteViolencia>>> Get()
        {
            return await _context.ReportesViolencia.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReporteViolencia>> Get(int id)
        {
            var reporte = await _context.ReportesViolencia.FindAsync(id);

            if (reporte == null)
                return NotFound();

            return reporte;
        }

        [HttpPost]
        public async Task<ActionResult<ReporteViolencia>> Post(ReporteViolencia reporte)
        {
            reporte.FechaReporte = DateTime.Now;

            _context.ReportesViolencia.Add(reporte);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get),
                new { id = reporte.IdReporteViolencia },
                reporte);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ReporteViolencia reporte)
        {
            if (id != reporte.IdReporteViolencia)
                return BadRequest();

            _context.Entry(reporte).State =
                EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var reporte =
                await _context.ReportesViolencia.FindAsync(id);

            if (reporte == null)
                return NotFound();

            _context.ReportesViolencia.Remove(reporte);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}