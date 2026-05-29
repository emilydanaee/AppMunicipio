using Microsoft.AspNetCore.Mvc;

namespace WebMunicipio.Controllers
{
    public class IngresoController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
    }
}
