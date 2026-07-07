using ApiMunicipio.Data;
using ApiMunicipio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMunicipio.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PermisoController : ControllerBase
{
    private readonly MunicipioContext _context;

    public PermisoController(MunicipioContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Permiso>>> GetPermisos()
    {
        return Ok(await _context.Permisos.AsNoTracking()
            .OrderByDescending(p => p.FechaPermiso)
            .ToListAsync());
    }

    [HttpGet("usuario/{cedula}")]
    public async Task<ActionResult<List<Permiso>>> GetPermisosUsuario(string cedula)
    {
        var marker = Marker(cedula);
        return Ok(await _context.Permisos.AsNoTracking()
            .Where(p => p.Observaciones.Contains(marker))
            .OrderByDescending(p => p.FechaPermiso)
            .ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Permiso>> GetPermisoById(int id)
    {
        var permiso = await _context.Permisos.AsNoTracking().FirstOrDefaultAsync(p => p.IdPermiso == id);
        return permiso is null
            ? NotFound(new { message = "La solicitud no existe." })
            : Ok(permiso);
    }

    [HttpPost]
    public async Task<ActionResult<Permiso>> AddPermiso(Permiso newPermiso)
    {
        if (string.IsNullOrWhiteSpace(newPermiso.TipoPermiso) ||
            string.IsNullOrWhiteSpace(newPermiso.DescripcionPermiso))
            return BadRequest(new { message = "Completa el tipo y la descripción del trámite." });

        newPermiso.IdPermiso = 0;
        newPermiso.FechaPermiso = DateTime.Now;
        newPermiso.EstadoPermiso = string.IsNullOrWhiteSpace(newPermiso.EstadoPermiso)
            ? "Ingresado"
            : newPermiso.EstadoPermiso;
        newPermiso.DocumentoAdjunto ??= string.Empty;
        newPermiso.Observaciones ??= string.Empty;

        _context.Permisos.Add(newPermiso);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPermisoById), new { id = newPermiso.IdPermiso }, newPermiso);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePermiso(int id, Permiso permisoActualizado)
    {
        var permiso = await _context.Permisos.FindAsync(id);
        if (permiso is null)
            return NotFound(new { message = "La solicitud no existe." });

        permiso.TipoPermiso = permisoActualizado.TipoPermiso;
        permiso.DescripcionPermiso = permisoActualizado.DescripcionPermiso;
        permiso.EstadoPermiso = permisoActualizado.EstadoPermiso;
        permiso.DocumentoAdjunto = permisoActualizado.DocumentoAdjunto;
        permiso.Observaciones = permisoActualizado.Observaciones;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePermiso(int id)
    {
        var permiso = await _context.Permisos.FindAsync(id);
        if (permiso is null)
            return NotFound(new { message = "La solicitud no existe." });
        if (!CanCancel(permiso.EstadoPermiso))
            return BadRequest(new { message = "Este trámite ya no puede cancelarse por su estado actual." });

        _context.Permisos.Remove(permiso);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private static string Marker(string cedula) => $"[CEDULA:{cedula.Trim()}]";
    private static bool CanCancel(string? status)
    {
        var value = status?.ToLowerInvariant() ?? string.Empty;
        return value.Contains("ingres") || value.Contains("pend") || value.Contains("solicit");
    }
}
