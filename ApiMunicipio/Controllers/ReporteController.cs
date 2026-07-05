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
    public class ReporteController : ControllerBase
    {
        private readonly MunicipioContext _context;

        public ReporteController(MunicipioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Reporte>>> GetReportes()
        {
            return Ok(await _context.Reportes.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reporte>> GetReporteByID(int id)
        {
            var reporte = await _context.Reportes.FindAsync(id);

            if (reporte == null)
                return NotFound();

            return Ok(reporte);
        }

        [HttpPost]
        public async Task<ActionResult<Reporte>> AddReporte([FromForm] ReporteDTO dto)
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
            var nuevoReporte = new Reporte
            {
                TipoReporte = dto.TipoReporte,
                DescripcionReporte = dto.DescripcionReporte,
                FechaReporte = DateTime.Now,
                AdministracionZonal = dto.AdministracionZonal,
                Parroquia = dto.Parroquia,
                Latitud = dto.Latitud,
                Longitud = dto.Longitud,
                Direccion=dto.Direccion,
                Cedula = dto.Cedula,
                Correo = dto.Correo,
                Telefono = dto.Telefono,
                EstadoReporte = "Pendiente",
                ImagenReporte = rutaImagen
            };
;
            Console.WriteLine(dto.Latitud);
            Console.WriteLine(dto.Longitud);

            Console.WriteLine(nuevoReporte.Latitud);
            Console.WriteLine(nuevoReporte.Longitud);

            _context.Reportes.Add(nuevoReporte);

            await _context.SaveChangesAsync();

            Console.WriteLine(nuevoReporte.Latitud);
            Console.WriteLine(nuevoReporte.Longitud);


            var guardado = await _context.Reportes
            .OrderByDescending(x => x.IdReporte)
            .FirstAsync();

            Console.WriteLine($"BD Latitud: {guardado.Latitud}");
            Console.WriteLine($"BD Longitud: {guardado.Longitud}");

            return CreatedAtAction(
                nameof(GetReporteByID),
                new { id = nuevoReporte.IdReporte },
                nuevoReporte);

            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReporte(int id, ReporteDTO reporte)
        {
            if (id != reporte.IdReporte)
                return BadRequest();

            _context.Entry(reporte).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReporte(int id)
        {
            var reporte = await _context.Reportes.FindAsync(id);

            if (reporte == null)
                return NotFound();

            _context.Reportes.Remove(reporte);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> ActualizarEstado(int id, EstadoReporteDTO dto)
        {
            var reporte = await _context.Reportes.FindAsync(id);

            if (reporte == null)
                return NotFound();

            reporte.EstadoReporte = dto.EstadoReporte;

            await _context.SaveChangesAsync();

            return Ok(reporte);
        }
    }
}
