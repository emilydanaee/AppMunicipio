using ApiMunicipio.Data;
using ApiMunicipio.DTO;
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
        public async Task<ActionResult<Feria>> AddFeria([FromForm] FeriaDTO dto)
        {
            string rutaImagen = string.Empty;

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
            var nuevoFeria = new Feria
            {
                NombreFeria = dto.NombreFeria,
                DescripcionFeria = dto.DescripcionFeria,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                HoraInicio = dto.HoraInicio,
                HoraFin = dto.HoraFin,
                SectorFeria = dto.SectorFeria,
                UbicacionFeria = dto.UbicacionFeria,
                ImagenFeria = rutaImagen
            };

            _context.Ferias.Add(nuevoFeria);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetFeriaByID),
                new { id = nuevoFeria.IdFeria },
                nuevoFeria);
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
