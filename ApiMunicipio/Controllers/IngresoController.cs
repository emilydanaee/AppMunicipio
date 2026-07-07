using ApiMunicipio.Data;
using ApiMunicipio.DTO;
using ApiMunicipio.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMunicipio.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IngresoController : ControllerBase
{
    private readonly UserManager<Usuario> _userManager;
    private readonly MunicipioContext _context;

    public IngresoController(UserManager<Usuario> userManager, MunicipioContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    [HttpPost("registro")]
    public async Task<IActionResult> Registro(RegistroDTO model)
    {
        var email = model.Email.Trim().ToLowerInvariant();
        var cedula = model.Cedula.Trim();
        var nombre = model.NombreCompleto.Trim();
        var sector = model.Sector.Trim();

        if (cedula.Length != 10 || !cedula.All(char.IsDigit))
            return BadRequest(new { message = "La cédula debe tener exactamente 10 dígitos." });

        if (await _userManager.FindByEmailAsync(email) is not null)
            return BadRequest(new { message = "Ya existe una cuenta con ese correo." });

        if (await _userManager.Users.AnyAsync(user => user.Cedula == cedula))
            return BadRequest(new { message = "Ya existe una cuenta con esa cédula." });

        var user = new Usuario
        {
            UserName = email,
            Email = email,
            NombreCompleto = nombre,
            Cedula = cedula,
            Sector = sector,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            var message = string.Join(" ", result.Errors.Select(error => error.Description));
            return BadRequest(new { message });
        }

        await _userManager.AddToRoleAsync(user, "Usuario");
        await EnsureTaxStatusAsync(user.Cedula);
        var roles = await _userManager.GetRolesAsync(user);

        return Ok(ToSession(user, roles));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO model)
    {
        var email = model.Email.Trim().ToLowerInvariant();
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return Unauthorized(new { message = "Usuario no existe. Revisa el correo o crea una cuenta." });

        if (!await _userManager.CheckPasswordAsync(user, model.Password))
            return Unauthorized(new { message = "La contraseña es incorrecta." });

        await EnsureTaxStatusAsync(user.Cedula);
        var roles = await _userManager.GetRolesAsync(user);
        return Ok(ToSession(user, roles));
    }

    [HttpGet("perfil/{id}")]
    public async Task<IActionResult> Perfil(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
            return NotFound(new { message = "No se encontró el usuario." });

        var roles = await _userManager.GetRolesAsync(user);
        return Ok(ToSession(user, roles));
    }

    private async Task EnsureTaxStatusAsync(string cedula)
    {
        if (string.IsNullOrWhiteSpace(cedula) || await _context.Consultas.AnyAsync(c => c.Cedula == cedula))
            return;

        _context.Consultas.Add(new Consulta
        {
            Cedula = cedula,
            ValorPendiente = 0,
            Pagado = true,
            FechaConsulta = DateTime.Now
        });
        await _context.SaveChangesAsync();
    }

    private static UsuarioSesionDTO ToSession(Usuario user, IEnumerable<string> roles) => new()
    {
        Id = user.Id,
        NombreCompleto = user.NombreCompleto,
        Email = user.Email ?? string.Empty,
        Cedula = user.Cedula,
        Sector = user.Sector,
        Roles = roles.ToList()
    };
}
