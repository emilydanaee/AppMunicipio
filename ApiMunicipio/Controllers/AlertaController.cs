using ApiMunicipio.Data;
using ApiMunicipio.DTO;
using ApiMunicipio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMunicipio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertaController : ControllerBase
    {
        private readonly MunicipioContext _context;

        public AlertaController(MunicipioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Alerta>>> GetAlertas()
        {
            return Ok(await _context.Alertas.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Alerta>> GetAlerta(int id)
        {
            var alerta = await _context.Alertas.FindAsync(id);

            if (alerta == null)
                return NotFound();

            return Ok(alerta);
        }

        [HttpPost]
        public async Task<ActionResult<Alerta>> CrearAlerta([FromForm] AlertaDTO dto)
        {
            string? rutaImagen = null;

            if (dto.Imagen != null)
            {
                string nombre = Guid.NewGuid() +
                                Path.GetExtension(dto.Imagen.FileName);

                string carpeta = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images");

                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                string ruta = Path.Combine(carpeta, nombre);

                using var stream = new FileStream(ruta, FileMode.Create);

                await dto.Imagen.CopyToAsync(stream);

                rutaImagen = "images/" + nombre;
            }

            var alerta = new Alerta
            {
                TipoAlerta = dto.TipoAlerta,
                DescripcionAlerta = dto.DescripcionAlerta,
                Fecha = DateTime.Now,
                Sector = dto.Sector,
                Latitud = dto.Latitud,
                Longitud = dto.Longitud,
                Direccion = dto.Direccion,
                Cedula = dto.Cedula,
                Correo = dto.Correo,
                Telefono = dto.Telefono,
                EstadoAlerta = "Activa",
                ImagenAlerta = rutaImagen
            };

            _context.Alertas.Add(alerta);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAlerta),
                new { id = alerta.IdAlerta },
                alerta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarEstado(int id, Alerta alertaActualizada)
        {
            var alerta = await _context.Alertas.FindAsync(id);

            if (alerta == null)
                return NotFound();

            alerta.EstadoAlerta = alertaActualizada.EstadoAlerta;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var alerta = await _context.Alertas.FindAsync(id);

            if (alerta == null)
                return NotFound();

            _context.Alertas.Remove(alerta);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}