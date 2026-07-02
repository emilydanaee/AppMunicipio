using ApiMunicipio.Data;
using ApiMunicipio.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMunicipio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisoController : ControllerBase
    {
        private readonly MunicipioContext _context;

        public PermisoController(MunicipioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Permiso>>> GetPermisos()
        {
            return Ok(await _context.Permisos.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Permiso>> GetPermisoByID(int id)
        {
            var permiso = await _context.Permisos.FindAsync(id);

            if (permiso == null)
                return NotFound();

            return Ok(permiso);
        }

        [HttpPost]
        public async Task<ActionResult<Permiso>> AddPermiso(Permiso newPermiso)
        {
            _context.Permisos.Add(newPermiso);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPermisoByID),
                new { id = newPermiso.IdPermiso }, newPermiso);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePermiso(int id, Permiso permiso)
        {
            if (id != permiso.IdPermiso)
                return BadRequest();

            _context.Entry(permiso).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermiso(int id)
        {
            var permiso = await _context.Permisos.FindAsync(id);

            if (permiso == null)
                return NotFound();

            _context.Permisos.Remove(permiso);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
