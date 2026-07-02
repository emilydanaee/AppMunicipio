using ApiMunicipio.Data;
using ApiMunicipio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMunicipio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SituacionCalleController : ControllerBase
    {
        private readonly MunicipioContext _context;

        public SituacionCalleController(MunicipioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SituacionCalle>>> GetSituacionCalle()
        {
            return Ok(await _context.SituacionCalle.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SituacionCalle>> GetSituacionCalleByID(int id)
        {
            var situacionCalle = await _context.SituacionCalle.FindAsync(id);

            if (situacionCalle == null)
                return NotFound();

            return Ok(situacionCalle);
        }

        [HttpPost]
        public async Task<ActionResult<SituacionCalle>> AddSituacionCalle(SituacionCalle newSituacionCalle)
        {
            if (newSituacionCalle == null)
                return BadRequest();
            _context.SituacionCalle.Add(newSituacionCalle);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSituacionCalleByID),
                new { id = newSituacionCalle.IdSituacionCalle }, newSituacionCalle);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSituacionCalle(int id, SituacionCalle situacionCalle)
        {
            if (id != situacionCalle.IdSituacionCalle)
                return BadRequest();

            _context.Entry(situacionCalle).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSituacionCalle(int id)
        {
            var situacionCalle = await _context.SituacionCalle.FindAsync(id);

            if (situacionCalle == null)
                return NotFound();

            _context.SituacionCalle.Remove(situacionCalle);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
