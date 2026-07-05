using ApiMunicipio.Data;
using ApiMunicipio.DTO;
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
        public async Task<ActionResult<Taller>> AddTaller([FromForm] TallerDTO dto)
        {
            string? rutaImagen = null;

            // Guardar imagen si existe
            if (dto.Imagen != null && dto.Imagen.Length > 0)
            {
                // Nombre único para evitar archivos repetidos
                string nombreArchivo = Guid.NewGuid().ToString() +
                                       Path.GetExtension(dto.Imagen.FileName);

                // Ruta física
                string carpeta = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images");

                // Crear carpeta si no existe
                if (!Directory.Exists(carpeta))
                {
                    Directory.CreateDirectory(carpeta);
                }

                string rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await dto.Imagen.CopyToAsync(stream);
                }

                // Ruta que se guardará en la BD
                rutaImagen = "images/" + nombreArchivo;
            }

            // Crear entidad
            var nuevoTaller = new Taller
            {
                NombreTaller = dto.NombreTaller,
                DescripcionTaller = dto.DescripcionTaller,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                HoraInicio = dto.HoraInicio,
                HoraFin = dto.HoraFin,
                SectorTaller = dto.SectorTaller,
                UbicacionTaller = dto.UbicacionTaller,
                Instructor = dto.Instructor,
                Dias = dto.Dias,
                Modalidad = dto.Modalidad,
                CuposTaller = dto.CuposTaller,
                ImagenTaller = rutaImagen
            };

            _context.Talleres.Add(nuevoTaller);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTallerByID),
                new { id = nuevoTaller.IdTaller },
                nuevoTaller);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTaller(int id, Taller taller)
        {
            if (id != taller.IdTaller)
                return BadRequest();

            _context.Entry(taller).State = EntityState.Modified;

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

        [HttpPost("Inscribirse")]
        public async Task<IActionResult> Inscribirse(
    InscripcionTallerDTO dto)
        {
            // Buscar el taller

            var taller = await _context.Talleres
                .FindAsync(dto.IdTaller);

            if (taller == null)
            {
                return NotFound("El taller no existe.");
            }

            // Verificar cupos

            if (taller.Inscritos >= taller.CuposTaller)
            {
                return BadRequest("No existen cupos disponibles.");
            }

            // Verificar si la persona ya está inscrita

            bool existe = await _context.InscripcionesTaller.AnyAsync(i =>
                i.IdTaller == dto.IdTaller &&
                i.Cedula == dto.Cedula);

            if (existe)
            {
                return BadRequest("La persona ya se encuentra inscrita.");
            }

            // Crear inscripción

            var inscripcion = new InscripcionTaller
            {
                IdTaller = dto.IdTaller,
                Nombre = dto.Nombre,
                Cedula = dto.Cedula,
                Correo = dto.Correo,
                Telefono = dto.Telefono,
                Fecha = DateTime.Now
            };

            _context.InscripcionesTaller.Add(inscripcion);

            // Incrementar inscritos

            taller.Inscritos++;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Inscripción realizada correctamente."
            });
        }

        [HttpGet("{id}/Inscritos")]
        public async Task<ActionResult<IEnumerable<InscripcionTaller>>> GetInscritos(int id)
        {
            var inscritos = await _context.InscripcionesTaller
                .Where(i => i.IdTaller == id)
                .OrderBy(i => i.Nombre)
                .ToListAsync();

            return Ok(inscritos);
        }

        [HttpDelete("Cancelar/{id}")]
        public async Task<IActionResult> CancelarInscripcion(int id)
        {
            var inscripcion = await _context.InscripcionesTaller
                .FindAsync(id);

            if (inscripcion == null)
                return NotFound();

            var taller = await _context.Talleres
                .FindAsync(inscripcion.IdTaller);

            if (taller != null && taller.Inscritos > 0)
            {
                taller.Inscritos--;
            }

            _context.InscripcionesTaller.Remove(inscripcion);

            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
