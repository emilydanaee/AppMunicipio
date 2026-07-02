using ApiMunicipio.DTO;
using ApiMunicipio.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiMunicipio.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class IngresoController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
       
        public IngresoController(UserManager<Usuario> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registro(RegistroDTO model)
        {
            // Verificar si el usuario ya existe
            var existingUser = await _userManager.FindByEmailAsync(model.Email);

            if (existingUser != null)
                return BadRequest("El usuario ya existe");

            var user = new Usuario
            {
                UserName = model.Email,
                Email = model.Email,
                NombreCompleto = model.NombreCompleto,
                Cedula = model.Cedula,   // 🔴 ESTO FALTABA O ESTÁ FALLANDO
                Sector = model.Sector
                // Si quieres luego puedes guardar Cedula y Sector en el modelo Usuario
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "Usuario");

            return Ok(new
            {
                mensaje = "Usuario registrado correctamente"
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                return BadRequest("Datos incompletos");

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return Unauthorized("Usuario no existe");

            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!passwordValid)
                return Unauthorized("Credenciales incorrectas");

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new UsuarioSesionDTO
            {
                Id = user.Id,
                NombreCompleto = user.NombreCompleto,
                Email = user.Email,
                Cedula = user.Cedula,
                Sector = user.Sector,
                Roles = roles.ToList()
            });
        }
    }
}
