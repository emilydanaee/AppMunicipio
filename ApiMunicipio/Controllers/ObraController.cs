using ApiMunicipio.Data;
using ApiMunicipio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMunicipio.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ObraController : ControllerBase
{
    private readonly MunicipioContext _context;

    public ObraController(MunicipioContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Obra>>> GetObras()
    {
        return Ok(await _context.Obras.AsNoTracking()
            .OrderByDescending(o => o.FechaSolicitud)
            .ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Obra>> GetObra(int id)
    {
        var obra = await _context.Obras.AsNoTracking()
            .FirstOrDefaultAsync(o => o.IdObra == id);
        return obra is null
            ? NotFound(new { message = "La propuesta de obra no existe." })
            : Ok(obra);
    }

    [HttpPost]
    public async Task<ActionResult<Obra>> CrearObra(Obra obra)
    {
        if (string.IsNullOrWhiteSpace(obra.NombreObra) ||
            string.IsNullOrWhiteSpace(obra.DescripcionObra) ||
            string.IsNullOrWhiteSpace(obra.SectorObra))
        {
            return BadRequest(new { message = "Completa el nombre, la descripción y el sector de la obra." });
        }

        obra.IdObra = 0;
        obra.FechaSolicitud = DateTime.Now;
        obra.EstadoObra = string.IsNullOrWhiteSpace(obra.EstadoObra)
            ? "Propuesta"
            : obra.EstadoObra;

        _context.Obras.Add(obra);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetObra), new { id = obra.IdObra }, obra);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> ActualizarObra(int id, Obra actualizada)
    {
        var obra = await _context.Obras.FindAsync(id);
        if (obra is null)
            return NotFound(new { message = "La propuesta de obra no existe." });

        obra.NombreObra = actualizada.NombreObra;
        obra.DescripcionObra = actualizada.DescripcionObra;
        obra.SectorObra = actualizada.SectorObra;
        obra.Latitud = actualizada.Latitud;
        obra.Longitud = actualizada.Longitud;
        obra.EstadoObra = string.IsNullOrWhiteSpace(actualizada.EstadoObra)
            ? obra.EstadoObra
            : actualizada.EstadoObra;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> EliminarObra(int id)
    {
        var obra = await _context.Obras.FindAsync(id);
        if (obra is null)
            return NotFound(new { message = "La propuesta de obra no existe." });

        _context.Obras.Remove(obra);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
