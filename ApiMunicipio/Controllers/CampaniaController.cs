using ApiMunicipio.Data;
using ApiMunicipio.DTO;
using ApiMunicipio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMunicipio.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CampaniaController : ControllerBase
{
    private readonly MunicipioContext _context;

    public CampaniaController(MunicipioContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Campania>>> GetCampanias()
    {
        return Ok(await _context.Campanias.AsNoTracking()
            .OrderBy(c => c.FechaInicio)
            .ThenBy(c => c.HoraInicio)
            .ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Campania>> GetCampaniaById(int id)
    {
        var campania = await _context.Campanias.AsNoTracking()
            .FirstOrDefaultAsync(c => c.IdCampania == id);
        return campania is null
            ? NotFound(new { message = "La campaña no existe." })
            : Ok(campania);
    }

    [HttpPost]
    public async Task<ActionResult<Campania>> AddCampania([FromForm] CampaniaDTO dto)
    {
        if (dto.FechaFin < dto.FechaInicio)
            return BadRequest(new { message = "La fecha final no puede ser anterior a la fecha inicial." });
        if (dto.HoraFin <= dto.HoraInicio)
            return BadRequest(new { message = "La hora final debe ser posterior a la hora inicial." });

        var campaign = new Campania
        {
            NombreCampania = dto.NombreCampania.Trim(),
            TipoCampania = dto.TipoCampania.Trim(),
            DescripcionCampania = dto.DescripcionCampania.Trim(),
            FechaInicio = dto.FechaInicio,
            FechaFin = dto.FechaFin,
            HoraInicio = dto.HoraInicio,
            HoraFin = dto.HoraFin,
            UbicacionCampania = dto.UbicacionCampania.Trim(),
            ResponsableCampania = dto.ResponsableCampania.Trim(),
            ImagenCampania = await SaveImageAsync(dto.Imagen)
        };

        _context.Campanias.Add(campaign);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCampaniaById), new { id = campaign.IdCampania }, campaign);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCampania(int id, Campania updated)
    {
        var campaign = await _context.Campanias.FindAsync(id);
        if (campaign is null)
            return NotFound(new { message = "La campaña no existe." });

        campaign.NombreCampania = updated.NombreCampania;
        campaign.TipoCampania = updated.TipoCampania;
        campaign.DescripcionCampania = updated.DescripcionCampania;
        campaign.FechaInicio = updated.FechaInicio;
        campaign.FechaFin = updated.FechaFin;
        campaign.HoraInicio = updated.HoraInicio;
        campaign.HoraFin = updated.HoraFin;
        campaign.UbicacionCampania = updated.UbicacionCampania;
        campaign.ResponsableCampania = updated.ResponsableCampania;
        campaign.ImagenCampania = updated.ImagenCampania;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCampania(int id)
    {
        var campaign = await _context.Campanias.FindAsync(id);
        if (campaign is null)
            return NotFound(new { message = "La campaña no existe." });

        _context.Campanias.Remove(campaign);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private static async Task<string?> SaveImageAsync(IFormFile? image)
    {
        if (image is null || image.Length == 0)
            return null;

        var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "campania");
        Directory.CreateDirectory(folder);
        var fileName = $"{Guid.NewGuid():N}{Path.GetExtension(image.FileName)}";
        await using var stream = System.IO.File.Create(Path.Combine(folder, fileName));
        await image.CopyToAsync(stream);
        return $"uploads/campania/{fileName}";
    }
}
