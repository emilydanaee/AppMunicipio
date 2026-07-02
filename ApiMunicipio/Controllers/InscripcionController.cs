using ApiMunicipio.Data;
using ApiMunicipio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMunicipio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InscripcionController : ControllerBase
    {
        private readonly MunicipioContext _context;

        public InscripcionController(MunicipioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inscripcion>>> GetInscripciones()
        {
            return Ok(await _context.Ferias.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Inscripcion>> GetInscripcionByID(int id)
        {
            var inscripcion = await _context.Inscripciones.FindAsync(id);

            if (inscripcion == null)
                return NotFound();

            return Ok(inscripcion);
        }

        [HttpPost]
        public async Task<ActionResult<Inscripcion>> AddInscripcion(Inscripcion newInscripcion)
        {
            if (newInscripcion == null) return BadRequest();

            _context.Inscripciones.Add(newInscripcion);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInscripcionByID),
                new { id = newInscripcion.IdInscripcion }, newInscripcion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInscripcion(int id, Inscripcion inscripcion)
        {
            if (id != inscripcion.IdInscripcion)
                return BadRequest();

            _context.Entry(inscripcion).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeria(int id)
        {
            var feria = await _context.Ferias.FindAsync(id);

            if (feria == null)
                return NotFound();

            _context.Ferias.Remove(feria);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
