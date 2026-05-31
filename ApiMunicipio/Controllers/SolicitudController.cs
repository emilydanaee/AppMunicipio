using ApiMunicipio.Data;
using ApiMunicipio.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMunicipio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudController : ControllerBase
    {
        private readonly MunicipioContext _context;

        public SolicitudController(MunicipioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Solicitud>>> GetSolicitudes()
        {
            return Ok(await _context.Solicitudes.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Solicitud>> GetSolicitudByID(int id)
        {
            var solicitud = await _context.Solicitudes.FindAsync(id);

            if (solicitud == null)
                return NotFound();

            return Ok(solicitud);
        }

        [HttpPost]
        public async Task<ActionResult<Solicitud>> AddSolicitud(Solicitud newSolicitud)
        {
            _context.Solicitudes.Add(newSolicitud);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSolicitudByID),
                new { id = newSolicitud.IdSolicitud }, newSolicitud);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSolicitud(int id, Solicitud solicitud)
        {
            if (id != solicitud.IdSolicitud)
                return BadRequest();

            _context.Entry(solicitud).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSolicitud(int id)
        {
            var solicitud = await _context.Solicitudes.FindAsync(id);

            if (solicitud == null)
                return NotFound();

            _context.Solicitudes.Remove(solicitud);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
