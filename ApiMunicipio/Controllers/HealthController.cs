using ApiMunicipio.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMunicipio.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class HealthController : ControllerBase
{
    private readonly MunicipioContext _context;

    public HealthController(MunicipioContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var connected = await _context.Database.CanConnectAsync();
            return connected
                ? Ok(new { status = "ok", database = "connected", utc = DateTime.UtcNow })
                : StatusCode(StatusCodes.Status503ServiceUnavailable,
                    new { status = "error", database = "disconnected" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable,
                new { status = "error", database = "disconnected", message = ex.Message });
        }
    }
}
