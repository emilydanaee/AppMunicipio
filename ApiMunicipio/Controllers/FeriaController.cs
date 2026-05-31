using ApiMunicipio.Data;
using ApiMunicipio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMunicipio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeriaController : ControllerBase
    {
        private readonly MunicipioContext _context;

        public FeriaController(MunicipioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Feria>>> GetFerias()
        {
            return Ok(await _context.Ferias.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Feria>> GetFeriaByID(int id)
        {
            var feria = await _context.Ferias.FindAsync(id);

            if (feria == null)
                return NotFound();

            return Ok(feria);
        }

        [HttpPost]
        public async Task<ActionResult<Feria>> AddFeria(Feria newFeria)
        {
            if (newFeria == null)
                return BadRequest();
            _context.Ferias.Add(newFeria);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFeriaByID),
                new { id = newFeria.IdFeria }, newFeria);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeria(int id, Feria feria)
        {
            if (id != feria.IdFeria)
                return BadRequest();

            _context.Entry(feria).State = EntityState.Modified;

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
