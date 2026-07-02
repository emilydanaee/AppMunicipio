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
        private readonly IImageService _imageService;

        public CampaniaController(MunicipioContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
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
        public async Task<ActionResult<Campania>> AddCampania(
    [FromForm] CampaniaDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string? rutaImagen =
                await _imageService.GuardarImagen(
                    dto.Imagen,
                    "campanias");

            Campania nuevaCampania = new Campania
            {
                NombreCampania = dto.NombreCampania,
                DescripcionCampania = dto.DescripcionCampania,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                SectorCampania = dto.SectorCampania,
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
