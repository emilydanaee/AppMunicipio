using ApiMunicipio.Data;
using ApiMunicipio.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMunicipio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TallerController : ControllerBase
    {
        private readonly MunicipioContext _context;
        public TallerController(MunicipioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Taller>>> GetTaller()
        {
            return Ok(await _context.Talleres.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Taller>> GetTallerByID(int id)
        {
            var taller = await _context.Talleres.FindAsync(id);
            if (taller == null)
                return NotFound();
            return Ok(taller);
        }

        [HttpPost]
        public async Task<ActionResult<Taller>> AddTaller(Taller newTaller)
        {
            if (newTaller == null)
                return BadRequest();

            _context.Talleres.Add(newTaller);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTallerByID), new { id = newTaller.IdTaller }, newTaller);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTaller(int id, Taller upTaller)
        {
            var taller = await _context.Talleres.FindAsync(id);
            if (taller == null)
                return NotFound();
            
            taller.IdTaller = upTaller.IdTaller;
            taller.NombreTaller = upTaller.NombreTaller;
            taller.DescripcionTaller = upTaller.DescripcionTaller;
            taller.FechaInicio = upTaller.FechaInicio;
            taller.FechaFin = upTaller.FechaFin;
            taller.SectorTaller = upTaller.SectorTaller;
            taller.CuposTaller = upTaller.CuposTaller;

            await _context.SaveChangesAsync();  

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaller(int id)
        {
            var taller = await _context.Talleres.FindAsync(id);
            if (taller == null)
                return NotFound();

            _context.Talleres.Remove(taller);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
