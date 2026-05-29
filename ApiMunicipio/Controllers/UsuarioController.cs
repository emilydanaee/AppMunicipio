using ApiMunicipio.DTO;
using ApiMunicipio.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiMunicipio.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
       
        public UsuarioController(UserManager<Usuario> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistroDTO model)
        {
            var user = new Usuario
            {
                UserName = model.Email,
                Email = model.Email,
                NombreCompleto = model.NombreCompleto
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // TODOS los usuarios son "Usuario" por defecto
            await _userManager.AddToRoleAsync(user, "Usuario");

            return Ok("Usuario registrado correctamente");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return Unauthorized("Usuario no existe");

            var result = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!result)
                return Unauthorized("Credenciales incorrectas");

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                message = "Login exitoso",
                user = new
                {
                    user.Id,
                    user.Email,
                    user.NombreCompleto,
                    roles
                }
            });
        }
    }
}
