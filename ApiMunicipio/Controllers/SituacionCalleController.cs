using ApiMunicipio.Data;
using ApiMunicipio.DTO;
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
        public async Task<ActionResult<List<SituacionCalle>>> GetSituaciones()
        {
            return Ok(await _context.SituacionCalle
                .OrderByDescending(x => x.FechaReporte)
                .ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SituacionCalle>> GetSituacion(int id)
        {
            var situacion = await _context.SituacionCalle.FindAsync(id);

            if (situacion == null)
                return NotFound();

            return Ok(situacion);
        }

        [HttpPost]
        public async Task<ActionResult<SituacionCalle>> CrearSituacion(
            SituacionCalleDTO dto)
        {
            var situacion = new SituacionCalle
            {
                NombreReportante = dto.NombreReportante,
                FechaReporte = DateTime.Now,
                Sector = dto.Sector,
                Latitud = dto.Latitud,
                Longitud = dto.Longitud,
                Direccion = dto.Direccion,
                Condicion = dto.Condicion,
                Descripcion = dto.Descripcion,
                Prioridad = dto.Prioridad,
                RiesgoInmediato = dto.RiesgoInmediato,

                Estado = "Pendiente"
            };

            _context.SituacionCalle.Add(situacion);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetSituacion),
                new { id = situacion.IdSituacionCalle },
                situacion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarSituacion(
            int id,
            SituacionCalleDTO dto)
        {
            var situacion = await _context.SituacionCalle.FindAsync(id);

            if (situacion == null)
                return NotFound();

            situacion.NombreReportante = dto.NombreReportante;
            situacion.Sector = dto.Sector;
            situacion.Latitud = dto.Latitud;
            situacion.Longitud = dto.Longitud;
            situacion.Direccion = dto.Direccion;
            situacion.Condicion = dto.Condicion;
            situacion.Descripcion = dto.Descripcion;
            situacion.Prioridad = dto.Prioridad;
            situacion.RiesgoInmediato = dto.RiesgoInmediato;
            situacion.Estado = dto.Estado;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ==========================================
        // ACTUALIZAR SOLO EL ESTADO
        // ==========================================

        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> ActualizarEstado(
            int id,
            [FromBody] EstadoDTO estado)
        {
            var situacion = await _context.SituacionCalle.FindAsync(id);

            if (situacion == null)
                return NotFound();

            situacion.Estado = estado.Estado;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ==========================================
        // ELIMINAR
        // ==========================================

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var situacion = await _context.SituacionCalle.FindAsync(id);

            if (situacion == null)
                return NotFound();

            _context.SituacionCalle.Remove(situacion);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    // DTO utilizado únicamente para cambiar el estado

    public class EstadoDTO
    {
        public string Estado { get; set; }
    }
}