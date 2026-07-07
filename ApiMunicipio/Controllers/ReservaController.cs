using ApiMunicipio.Data;
using ApiMunicipio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMunicipio.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservaController : ControllerBase
{
    private readonly MunicipioContext _context;

    public ReservaController(MunicipioContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reserva>>> GetReservas()
    {
        return Ok(await _context.Reservas.AsNoTracking()
            .OrderByDescending(r => r.FechaReserva)
            .ThenByDescending(r => r.HoraInicio)
            .ToListAsync());
    }

    [HttpGet("usuario/{cedula}")]
    public async Task<ActionResult<IEnumerable<Reserva>>> GetReservasUsuario(string cedula)
    {
        var marker = Marker(cedula);
        return Ok(await _context.Reservas.AsNoTracking()
            .Where(r => r.Observaciones.Contains(marker))
            .OrderByDescending(r => r.FechaReserva)
            .ToListAsync());
    }

    [HttpGet("disponibilidad")]
    public async Task<ActionResult<object>> ConsultarDisponibilidad(
        [FromQuery] string espacio,
        [FromQuery] DateOnly fecha,
        [FromQuery] TimeOnly inicio,
        [FromQuery] TimeOnly fin)
    {
        var ocupada = await ExisteCruceAsync(espacio, fecha, inicio, fin, null);
        return Ok(new { disponible = !ocupada });
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Reserva>> GetReservaById(int id)
    {
        var reserva = await _context.Reservas.AsNoTracking().FirstOrDefaultAsync(r => r.IdReserva == id);
        return reserva is null
            ? NotFound(new { message = "La reserva no existe." })
            : Ok(reserva);
    }

    [HttpPost]
    public async Task<ActionResult<Reserva>> AddReserva(Reserva newReserva)
    {
        var validation = Validate(newReserva);
        if (validation is not null)
            return BadRequest(new { message = validation });

        if (await ExisteCruceAsync(newReserva.EspacioReserva, newReserva.FechaReserva,
                newReserva.HoraInicio, newReserva.HoraFin, null))
        {
            return BadRequest(new { message = "El espacio ya está reservado dentro de ese horario." });
        }

        newReserva.IdReserva = 0;
        newReserva.EstadoReserva = string.IsNullOrWhiteSpace(newReserva.EstadoReserva)
            ? "Solicitada"
            : newReserva.EstadoReserva;
        newReserva.DocumentoAdjunto ??= string.Empty;
        newReserva.Observaciones ??= string.Empty;

        _context.Reservas.Add(newReserva);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetReservaById), new { id = newReserva.IdReserva }, newReserva);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateReserva(int id, Reserva reservaActualizada)
    {
        var reserva = await _context.Reservas.FindAsync(id);
        if (reserva is null)
            return NotFound(new { message = "La reserva no existe." });

        var validation = Validate(reservaActualizada);
        if (validation is not null)
            return BadRequest(new { message = validation });

        if (await ExisteCruceAsync(reservaActualizada.EspacioReserva, reservaActualizada.FechaReserva,
                reservaActualizada.HoraInicio, reservaActualizada.HoraFin, id))
        {
            return BadRequest(new { message = "El espacio ya está reservado dentro de ese horario." });
        }

        reserva.EspacioReserva = reservaActualizada.EspacioReserva;
        reserva.FechaReserva = reservaActualizada.FechaReserva;
        reserva.HoraInicio = reservaActualizada.HoraInicio;
        reserva.HoraFin = reservaActualizada.HoraFin;
        reserva.MotivoReserva = reservaActualizada.MotivoReserva;
        reserva.EstadoReserva = reservaActualizada.EstadoReserva;
        reserva.Observaciones = reservaActualizada.Observaciones;
        reserva.DocumentoAdjunto = reservaActualizada.DocumentoAdjunto;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteReserva(int id)
    {
        var reserva = await _context.Reservas.FindAsync(id);
        if (reserva is null)
            return NotFound(new { message = "La reserva no existe." });
        if (!CanCancel(reserva.EstadoReserva))
            return BadRequest(new { message = "Esta reserva ya no puede cancelarse por su estado actual." });

        _context.Reservas.Remove(reserva);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private async Task<bool> ExisteCruceAsync(
        string espacio,
        DateOnly fecha,
        TimeOnly inicio,
        TimeOnly fin,
        int? excluirId)
    {
        var normalized = espacio.Trim().ToLower();
        return await _context.Reservas.AsNoTracking().AnyAsync(r =>
            (!excluirId.HasValue || r.IdReserva != excluirId.Value) &&
            r.EspacioReserva.ToLower() == normalized &&
            r.FechaReserva == fecha &&
            !r.EstadoReserva.ToLower().Contains("cancel") &&
            inicio < r.HoraFin && fin > r.HoraInicio);
    }

    private static string? Validate(Reserva value)
    {
        if (string.IsNullOrWhiteSpace(value.EspacioReserva) ||
            string.IsNullOrWhiteSpace(value.MotivoReserva))
            return "Completa el espacio y el motivo de la reserva.";
        if (value.FechaReserva < DateOnly.FromDateTime(DateTime.Today))
            return "La fecha de reserva no puede estar en el pasado.";
        if (value.HoraFin <= value.HoraInicio)
            return "La hora final debe ser posterior a la hora inicial.";
        return null;
    }

    private static string Marker(string cedula) => $"[CEDULA:{cedula.Trim()}]";

    private static bool CanCancel(string? status)
    {
        var value = status?.ToLowerInvariant() ?? string.Empty;
        return value.Contains("ingres") || value.Contains("pend") || value.Contains("solicit");
    }
}
