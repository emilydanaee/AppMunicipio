using ApiMunicipio.Data;
using ApiMunicipio.DTO;
using ApiMunicipio.Models;
using ApiMunicipio.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMunicipio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaniaController : ControllerBase
    {
        private readonly MunicipioContext _context;

        public CampaniaController(MunicipioContext context, IImageService imageService)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Campania>>> GetCampanias()
        {
            return Ok(await _context.Campanias.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Campania>> GetCampaniaByID(int id)
        {
            var campania = await _context.Campanias.FindAsync(id);

            if (campania == null)
                return NotFound();

            return Ok(campania);
        }

        [HttpPost]
        public async Task<ActionResult<Campania>> AddCampania([FromForm] CampaniaDTO dto)
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

            var nuevaCampania = new Campania
            {
                NombreCampania = dto.NombreCampania,
                DescripcionCampania = dto.DescripcionCampania,
                TipoCampania = dto.TipoCampania,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                HoraInicio = dto.HoraInicio,
                HoraFin = dto.HoraFin,
                UbicacionCampania = dto.UbicacionCampania,
                ResponsableCampania = dto.ResponsableCampania,
                ImagenCampania = rutaImagen
            };

            _context.Campanias.Add(nuevaCampania);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetCampaniaByID),
                new { id = nuevaCampania.IdCampania },
                nuevaCampania);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCampania(int id, Campania campania)
        {
            if (id != campania.IdCampania)
                return BadRequest();

            _context.Entry(campania).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampania(int id)
        {
            var campania = await _context.Campanias.FindAsync(id);

            if (campania == null)
                return NotFound();

            _context.Campanias.Remove(campania);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
