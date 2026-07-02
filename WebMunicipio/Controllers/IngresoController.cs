using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using WebMunicipio.DTO;

namespace WebMunicipio.Controllers
{
    public class IngresoController : Controller
    {
        private readonly HttpClient _httpClient;

        public IngresoController()
        {
            _httpClient = new HttpClient();

            _httpClient.BaseAddress =
                new Uri("https://localhost:7061/");
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(RegistroDTO dto)
        {
            var response =
                await _httpClient.PostAsJsonAsync(
                    "api/Ingreso/registro",
                    dto);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }

            ViewBag.Error = "No se pudo registrar";

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var response =
                await _httpClient.PostAsJsonAsync(
                    "api/Ingreso/login",
                    dto);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error =
                    "Correo o contraseña incorrectos";

                return View(dto);
            }

            var usuario =  await response.Content.ReadFromJsonAsync<UsuarioSesionDTO>();

            HttpContext.Session.SetString( "Nombre", usuario.NombreCompleto);

            HttpContext.Session.SetString("Rol", usuario.Roles.FirstOrDefault() ?? "");

            HttpContext.Session.SetString("Id", usuario.Id);

            HttpContext.Session.SetString("Email", usuario.Email);

            HttpContext.Session.SetString("Sector", usuario.Sector);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(
                "Index",
                "Home");
        }
    }
}
