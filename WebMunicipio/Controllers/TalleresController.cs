using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebMunicipio.Services;

namespace WebMunicipio.Controllers
{
    public class TalleresController : Controller
    {
        private readonly TallerService _tallerService;

        public TalleresController(TallerService tallerService)
        {
            _tallerService = tallerService;
        }

        public async Task<IActionResult> Index()
        {
            var talleres = await _tallerService.ObtenerTalleres();

            return View(talleres);
        }
    }
}
