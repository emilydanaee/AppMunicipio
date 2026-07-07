using ApiMunicipio.Data;
using ApiMunicipio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMunicipio.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InscripcionController : ControllerBase
{
    private readonly MunicipioContext _context;

    public InscripcionController(MunicipioContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Inscripcion>>> GetInscripciones()
    {
        return Ok(await _context.Inscripciones.AsNoTracking()
            .OrderByDescending(i => i.FechaInscripcion)
            .ToListAsync());
    }

    [HttpGet("usuario/{cedula}")]
    public async Task<ActionResult<IEnumerable<Inscripcion>>> GetPorUsuario(string cedula)
    {
        return Ok(await _context.Inscripciones.AsNoTracking()
            .Where(i => i.Cedula == cedula)
            .OrderByDescending(i => i.FechaInscripcion)
            .ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Inscripcion>> GetInscripcionById(int id)
    {
        var inscripcion = await _context.Inscripciones.AsNoTracking()
            .FirstOrDefaultAsync(i => i.IdInscripcion == id);

        return inscripcion is null
            ? NotFound(new { message = "La inscripción no existe." })
            : Ok(inscripcion);
    }

    [HttpPost]
    public async Task<ActionResult<Inscripcion>> AddInscripcion(Inscripcion nueva)
    {
        if (string.IsNullOrWhiteSpace(nueva.NombreActividad) ||
            string.IsNullOrWhiteSpace(nueva.NombreCompleto) ||
            string.IsNullOrWhiteSpace(nueva.Cedula) ||
            string.IsNullOrWhiteSpace(nueva.Correo))
        {
            return BadRequest(new { message = "Completa la actividad y los datos del ciudadano." });
        }

        var duplicada = await _context.Inscripciones.AnyAsync(i =>
            i.Cedula == nueva.Cedula &&
            i.NombreActividad == nueva.NombreActividad &&
            i.Estado != "Cancelada");

        if (duplicada)
            return BadRequest(new { message = "Ya existe una inscripción activa para esta actividad." });

        nueva.IdInscripcion = 0;
        nueva.FechaInscripcion = DateTime.Now;
        nueva.Estado = string.IsNullOrWhiteSpace(nueva.Estado) ? "Activa" : nueva.Estado;
        nueva.Telefono ??= string.Empty;
        nueva.DescripcionActividad ??= string.Empty;

        _context.Inscripciones.Add(nueva);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetInscripcionById),
            new { id = nueva.IdInscripcion }, nueva);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateInscripcion(int id, Inscripcion actualizada)
    {
        var existente = await _context.Inscripciones.FindAsync(id);
        if (existente is null)
            return NotFound(new { message = "La inscripción no existe." });

        existente.NombreActividad = actualizada.NombreActividad;
        existente.DescripcionActividad = actualizada.DescripcionActividad;
        existente.NombreCompleto = actualizada.NombreCompleto;
        existente.Cedula = actualizada.Cedula;
        existente.Correo = actualizada.Correo;
        existente.Telefono = actualizada.Telefono;
        existente.Estado = string.IsNullOrWhiteSpace(actualizada.Estado)
            ? existente.Estado
            : actualizada.Estado;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteInscripcion(int id)
    {
        var inscripcion = await _context.Inscripciones.FindAsync(id);
        if (inscripcion is null)
            return NotFound(new { message = "La inscripción no existe." });

        _context.Inscripciones.Remove(inscripcion);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
