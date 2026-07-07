using ApiMunicipio.Data;
using ApiMunicipio.DTO;
using ApiMunicipio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMunicipio.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TallerController : ControllerBase
{
    private readonly MunicipioContext _context;

    public TallerController(MunicipioContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Taller>>> GetTalleres()
    {
        return Ok(await _context.Talleres
            .AsNoTracking()
            .OrderBy(t => t.FechaInicio)
            .ThenBy(t => t.HoraInicio)
            .ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Taller>> GetTallerById(int id)
    {
        var taller = await _context.Talleres.AsNoTracking().FirstOrDefaultAsync(t => t.IdTaller == id);
        return taller is null
            ? NotFound(new { message = "El taller no existe." })
            : Ok(taller);
    }

    [HttpPost]
    public async Task<ActionResult<Taller>> AddTaller([FromForm] TallerDTO dto)
    {
        if (dto.FechaFin < dto.FechaInicio)
            return BadRequest(new { message = "La fecha final no puede ser anterior a la fecha inicial." });
        if (dto.HoraFin <= dto.HoraInicio)
            return BadRequest(new { message = "La hora final debe ser posterior a la hora inicial." });
        if (dto.CuposTaller <= 0)
            return BadRequest(new { message = "La cantidad de cupos debe ser mayor que cero." });

        var nuevoTaller = new Taller
        {
            NombreTaller = dto.NombreTaller.Trim(),
            DescripcionTaller = dto.DescripcionTaller.Trim(),
            FechaInicio = dto.FechaInicio,
            FechaFin = dto.FechaFin,
            HoraInicio = dto.HoraInicio,
            HoraFin = dto.HoraFin,
            SectorTaller = dto.SectorTaller.Trim(),
            UbicacionTaller = dto.UbicacionTaller.Trim(),
            Instructor = dto.Instructor.Trim(),
            Dias = dto.Dias.Trim(),
            Modalidad = dto.Modalidad.Trim(),
            CuposTaller = dto.CuposTaller,
            ImagenTaller = await SaveImageAsync(dto.Imagen)
        };

        _context.Talleres.Add(nuevoTaller);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTallerById), new { id = nuevoTaller.IdTaller }, nuevoTaller);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTaller(int id, Taller tallerActualizado)
    {
        var taller = await _context.Talleres.FindAsync(id);
        if (taller is null)
            return NotFound(new { message = "El taller no existe." });

        if (tallerActualizado.FechaFin < tallerActualizado.FechaInicio ||
            tallerActualizado.HoraFin <= tallerActualizado.HoraInicio)
            return BadRequest(new { message = "Revisa las fechas y horas del taller." });

        taller.NombreTaller = tallerActualizado.NombreTaller;
        taller.DescripcionTaller = tallerActualizado.DescripcionTaller;
        taller.FechaInicio = tallerActualizado.FechaInicio;
        taller.FechaFin = tallerActualizado.FechaFin;
        taller.HoraInicio = tallerActualizado.HoraInicio;
        taller.HoraFin = tallerActualizado.HoraFin;
        taller.SectorTaller = tallerActualizado.SectorTaller;
        taller.UbicacionTaller = tallerActualizado.UbicacionTaller;
        taller.Instructor = tallerActualizado.Instructor;
        taller.Dias = tallerActualizado.Dias;
        taller.Modalidad = tallerActualizado.Modalidad;
        taller.CuposTaller = tallerActualizado.CuposTaller;
        taller.ImagenTaller = tallerActualizado.ImagenTaller;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTaller(int id)
    {
        var taller = await _context.Talleres.FindAsync(id);
        if (taller is null)
            return NotFound(new { message = "El taller no existe." });

        var inscripciones = _context.InscripcionesTaller.Where(i => i.IdTaller == id);
        _context.InscripcionesTaller.RemoveRange(inscripciones);
        _context.Talleres.Remove(taller);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("Inscribirse")]
    public async Task<ActionResult<InscripcionResumenDTO>> Inscribirse([FromBody] InscripcionTallerDTO dto)
    {
        if (dto is null)
            return BadRequest(new { message = "No se recibieron los datos de inscripción." });

        var nombre = dto.Nombre?.Trim() ?? string.Empty;
        var cedula = dto.Cedula?.Trim() ?? string.Empty;
        var correo = dto.Correo?.Trim().ToLowerInvariant() ?? string.Empty;
        var telefono = dto.Telefono?.Trim() ?? string.Empty;

        if (dto.IdTaller <= 0)
            return BadRequest(new { message = "El taller seleccionado no es válido." });
        if (string.IsNullOrWhiteSpace(nombre))
            return BadRequest(new { message = "El nombre del usuario es obligatorio." });
        if (cedula.Length != 10 || !cedula.All(char.IsDigit))
            return BadRequest(new { message = "La cédula de la cuenta no es válida." });
        if (string.IsNullOrWhiteSpace(correo) || !correo.Contains('@'))
            return BadRequest(new { message = "El correo de la cuenta no es válido." });
        if (telefono.Count(char.IsDigit) < 7)
            return BadRequest(new { message = "Ingresa un teléfono de contacto válido." });

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var taller = await _context.Talleres
                .FirstOrDefaultAsync(t => t.IdTaller == dto.IdTaller);

            if (taller is null)
                return NotFound(new { message = "El taller no existe." });

            var existente = await _context.InscripcionesTaller
                .AsNoTracking()
                .AnyAsync(i => i.IdTaller == dto.IdTaller && i.Cedula == cedula);

            if (existente)
                return BadRequest(new { message = "Ya tienes un cupo reservado en este taller." });

            if (taller.CuposTaller > 0 && taller.Inscritos >= taller.CuposTaller)
                return BadRequest(new { message = "No existen cupos disponibles." });

            var inscripcion = new InscripcionTaller
            {
                IdTaller = dto.IdTaller,
                Nombre = nombre,
                Cedula = cedula,
                Correo = correo,
                Telefono = telefono,
                Fecha = DateTime.Now
            };

            _context.InscripcionesTaller.Add(inscripcion);
            taller.Inscritos = Math.Max(0, taller.Inscritos) + 1;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok(ToSummary(inscripcion, taller));
        }
        catch (DbUpdateException)
        {
            await transaction.RollbackAsync();
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "No se pudo guardar la inscripción en la base de datos." });
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "No se pudo completar la reserva del cupo." });
        }
    }

    [HttpGet("{id:int}/Inscripcion/{cedula}")]
    public async Task<ActionResult<InscripcionResumenDTO>> GetInscripcion(int id, string cedula)
    {
        var result = await (
            from inscripcion in _context.InscripcionesTaller.AsNoTracking()
            join taller in _context.Talleres.AsNoTracking()
                on inscripcion.IdTaller equals taller.IdTaller
            where inscripcion.IdTaller == id && inscripcion.Cedula == cedula
            select new InscripcionResumenDTO
            {
                IdInscripcion = inscripcion.Id,
                IdTaller = taller.IdTaller,
                NombreTaller = taller.NombreTaller,
                DescripcionTaller = taller.DescripcionTaller,
                FechaInicio = taller.FechaInicio,
                HoraInicio = taller.HoraInicio,
                HoraFin = taller.HoraFin,
                UbicacionTaller = taller.UbicacionTaller,
                SectorTaller = taller.SectorTaller,
                Modalidad = taller.Modalidad,
                FechaInscripcion = inscripcion.Fecha,
                Telefono = inscripcion.Telefono
            }).FirstOrDefaultAsync();

        return result is null
            ? NotFound(new { message = "No tienes una inscripción en este taller." })
            : Ok(result);
    }

    [HttpGet("MisInscripciones/{cedula}")]
    public async Task<ActionResult<IEnumerable<InscripcionResumenDTO>>> GetMisInscripciones(string cedula)
    {
        var results = await (
            from inscripcion in _context.InscripcionesTaller.AsNoTracking()
            join taller in _context.Talleres.AsNoTracking()
                on inscripcion.IdTaller equals taller.IdTaller
            where inscripcion.Cedula == cedula
            orderby taller.FechaInicio, taller.HoraInicio
            select new InscripcionResumenDTO
            {
                IdInscripcion = inscripcion.Id,
                IdTaller = taller.IdTaller,
                NombreTaller = taller.NombreTaller,
                DescripcionTaller = taller.DescripcionTaller,
                FechaInicio = taller.FechaInicio,
                HoraInicio = taller.HoraInicio,
                HoraFin = taller.HoraFin,
                UbicacionTaller = taller.UbicacionTaller,
                SectorTaller = taller.SectorTaller,
                Modalidad = taller.Modalidad,
                FechaInscripcion = inscripcion.Fecha,
                Telefono = inscripcion.Telefono
            }).ToListAsync();

        return Ok(results);
    }

    [HttpGet("{id:int}/Inscritos")]
    public async Task<ActionResult<IEnumerable<InscripcionTaller>>> GetInscritos(int id)
    {
        return Ok(await _context.InscripcionesTaller
            .AsNoTracking()
            .Where(i => i.IdTaller == id)
            .OrderBy(i => i.Nombre)
            .ToListAsync());
    }

    [HttpDelete("Cancelar/{id:int}")]
    public async Task<IActionResult> CancelarInscripcion(int id)
    {
        var inscripcion = await _context.InscripcionesTaller.FindAsync(id);
        if (inscripcion is null)
            return NotFound(new { message = "La inscripción no existe." });

        await RemoveRegistrationAsync(inscripcion);
        return NoContent();
    }

    [HttpDelete("Cancelar/{idTaller:int}/{cedula}")]
    public async Task<IActionResult> CancelarInscripcionPorCedula(int idTaller, string cedula)
    {
        var inscripcion = await _context.InscripcionesTaller
            .FirstOrDefaultAsync(i => i.IdTaller == idTaller && i.Cedula == cedula);
        if (inscripcion is null)
            return NotFound(new { message = "No se encontró la inscripción." });

        await RemoveRegistrationAsync(inscripcion);
        return NoContent();
    }

    private async Task RemoveRegistrationAsync(InscripcionTaller inscripcion)
    {
        var taller = await _context.Talleres.FindAsync(inscripcion.IdTaller);
        if (taller is not null && taller.Inscritos > 0)
            taller.Inscritos--;

        _context.InscripcionesTaller.Remove(inscripcion);
        await _context.SaveChangesAsync();
    }

    private static InscripcionResumenDTO ToSummary(InscripcionTaller inscripcion, Taller taller) => new()
    {
        IdInscripcion = inscripcion.Id,
        IdTaller = taller.IdTaller,
        NombreTaller = taller.NombreTaller,
        DescripcionTaller = taller.DescripcionTaller,
        FechaInicio = taller.FechaInicio,
        HoraInicio = taller.HoraInicio,
        HoraFin = taller.HoraFin,
        UbicacionTaller = taller.UbicacionTaller,
        SectorTaller = taller.SectorTaller,
        Modalidad = taller.Modalidad,
        FechaInscripcion = inscripcion.Fecha,
        Telefono = inscripcion.Telefono
    };

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
