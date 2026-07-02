using ApiMunicipio.Data;
using ApiMunicipio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMunicipio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultaController : ControllerBase
    {
        private readonly MunicipioContext _context;

        public ConsultaController(MunicipioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Consulta>>> GetConsultas()
        {
            return Ok(await _context.Consultas.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Consulta>> GetConsultaByID(int id)
        {
            var consulta = await _context.Consultas.FindAsync(id);

            if (consulta == null)
                return NotFound();

            return Ok(consulta);
        }

        [HttpPost]
        public async Task<ActionResult<Consulta>> AddConsulta(Consulta newConsulta)
        {
            if (newConsulta == null)
                return BadRequest();
            _context.Consultas.Add(newConsulta);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetConsultaByID),
                new { id = newConsulta.IdConsulta }, newConsulta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConsulta(int id, Consulta consulta)
        {
            if (id != consulta.IdConsulta)
                return BadRequest();

            _context.Entry(consulta).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConsulta(int id)
        {
            var consulta = await _context.Consultas.FindAsync(id);

            if (consulta == null)
                return NotFound();

            _context.Consultas.Remove(consulta);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
