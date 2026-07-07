using ApiMunicipio.Data;
using ApiMunicipio.DTO;
using ApiMunicipio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMunicipio.Controllers;

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
        return Ok(await _context.Reportes.AsNoTracking()
            .OrderByDescending(r => r.FechaReporte)
            .ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Reporte>> GetReporteById(int id)
    {
        var reporte = await _context.Reportes.AsNoTracking().FirstOrDefaultAsync(r => r.IdReporte == id);
        return reporte is null
            ? NotFound(new { message = "El reporte no existe." })
            : Ok(reporte);
    }

    [HttpGet("usuario/{cedula}")]
    public async Task<ActionResult<List<Reporte>>> GetReportesPorCedula(string cedula)
    {
        return Ok(await _context.Reportes.AsNoTracking()
            .Where(r => r.Cedula == cedula)
            .OrderByDescending(r => r.FechaReporte)
            .ToListAsync());
    }

    [HttpPost]
    public async Task<ActionResult<Reporte>> AddReporte([FromForm] ReporteDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.TipoReporte) || string.IsNullOrWhiteSpace(dto.DescripcionReporte))
            return BadRequest(new { message = "Selecciona el tipo de reporte y escribe una descripción." });
        if (string.IsNullOrWhiteSpace(dto.Direccion) && dto.Latitud == 0 && dto.Longitud == 0)
            return BadRequest(new { message = "Ingresa una dirección o adjunta la ubicación GPS." });
        if (string.IsNullOrWhiteSpace(dto.Cedula) || string.IsNullOrWhiteSpace(dto.Correo))
            return BadRequest(new { message = "Inicia sesión o activa el reporte anónimo." });

        var nuevoReporte = new Reporte
        {
            TipoReporte = dto.TipoReporte.Trim(),
            DescripcionReporte = dto.DescripcionReporte.Trim(),
            FechaReporte = DateTime.Now,
            AdministracionZonal = dto.AdministracionZonal?.Trim(),
            Parroquia = dto.Parroquia?.Trim(),
            Latitud = dto.Latitud,
            Longitud = dto.Longitud,
            Direccion = dto.Direccion?.Trim(),
            Cedula = dto.Cedula.Trim(),
            Correo = dto.Correo.Trim().ToLowerInvariant(),
            Telefono = dto.Telefono?.Trim(),
            EstadoReporte = "Pendiente",
            ImagenReporte = await SaveImageAsync(dto.Imagen)
        };

        _context.Reportes.Add(nuevoReporte);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetReporteById), new { id = nuevoReporte.IdReporte }, nuevoReporte);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateReporte(int id, [FromBody] ReporteDTO dto)
    {
        var reporte = await _context.Reportes.FindAsync(id);
        if (reporte is null)
            return NotFound(new { message = "El reporte no existe." });

        reporte.TipoReporte = dto.TipoReporte;
        reporte.DescripcionReporte = dto.DescripcionReporte;
        reporte.AdministracionZonal = dto.AdministracionZonal;
        reporte.Parroquia = dto.Parroquia;
        reporte.Latitud = dto.Latitud;
        reporte.Longitud = dto.Longitud;
        reporte.Direccion = dto.Direccion;
        reporte.Cedula = dto.Cedula;
        reporte.Correo = dto.Correo;
        reporte.Telefono = dto.Telefono;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteReporte(int id)
    {
        var reporte = await _context.Reportes.FindAsync(id);
        if (reporte is null)
            return NotFound(new { message = "El reporte no existe." });

        _context.Reportes.Remove(reporte);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id:int}/estado")]
    public async Task<IActionResult> ActualizarEstado(int id, EstadoReporteDTO dto)
    {
        var reporte = await _context.Reportes.FindAsync(id);
        if (reporte is null)
            return NotFound(new { message = "El reporte no existe." });

        reporte.EstadoReporte = dto.EstadoReporte;
        await _context.SaveChangesAsync();
        return Ok(reporte);
    }

    private static async Task<string?> SaveImageAsync(IFormFile? image)
    {
        if (image is null || image.Length == 0)
            return null;

        var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        Directory.CreateDirectory(folder);
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
        await using var stream = System.IO.File.Create(Path.Combine(folder, fileName));
        await image.CopyToAsync(stream);
        return $"images/{fileName}";
    }
}
