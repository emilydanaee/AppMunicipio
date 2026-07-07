using ApiMunicipio.Data;
using ApiMunicipio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMunicipio.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConsultaController : ControllerBase
{
    private readonly MunicipioContext _context;

    public ConsultaController(MunicipioContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Consulta>>> GetConsultas()
    {
        return Ok(await _context.Consultas.AsNoTracking()
            .OrderByDescending(c => c.FechaConsulta)
            .ToListAsync());
    }

    [HttpGet("cedula/{cedula}")]
    public async Task<ActionResult<IEnumerable<Consulta>>> GetPorCedula(string cedula)
    {
        return Ok(await _context.Consultas.AsNoTracking()
            .Where(c => c.Cedula == cedula)
            .OrderByDescending(c => c.FechaConsulta)
            .ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Consulta>> GetConsultaById(int id)
    {
        var consulta = await _context.Consultas.AsNoTracking().FirstOrDefaultAsync(c => c.IdConsulta == id);
        return consulta is null
            ? NotFound(new { message = "La consulta no existe." })
            : Ok(consulta);
    }

    [HttpPost]
    public async Task<ActionResult<Consulta>> AddConsulta(Consulta newConsulta)
    {
        if (string.IsNullOrWhiteSpace(newConsulta.Cedula))
            return BadRequest(new { message = "La cédula es obligatoria." });

        newConsulta.IdConsulta = 0;
        newConsulta.FechaConsulta = DateTime.Now;
        _context.Consultas.Add(newConsulta);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetConsultaById), new { id = newConsulta.IdConsulta }, newConsulta);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateConsulta(int id, Consulta consultaActualizada)
    {
        var consulta = await _context.Consultas.FindAsync(id);
        if (consulta is null)
            return NotFound(new { message = "La consulta no existe." });

        consulta.Cedula = consultaActualizada.Cedula;
        consulta.ValorPendiente = consultaActualizada.ValorPendiente;
        consulta.Pagado = consultaActualizada.Pagado;
        consulta.FechaConsulta = DateTime.Now;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteConsulta(int id)
    {
        var consulta = await _context.Consultas.FindAsync(id);
        if (consulta is null)
            return NotFound(new { message = "La consulta no existe." });

        _context.Consultas.Remove(consulta);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
