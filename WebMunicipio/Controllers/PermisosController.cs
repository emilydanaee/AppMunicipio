using Microsoft.AspNetCore.Mvc;
using WebMunicipio.Models;
using WebMunicipio.Services;

namespace WebMunicipio.Controllers
{
    public class PermisosController : Controller
    {
        private readonly PermisoService _permisoService;

        public PermisosController(PermisoService permisoService)
        {
            _permisoService = permisoService;
        }

        public async Task<IActionResult> Index()
        {
            var permisos = await _permisoService.ObtenerPermisos();

            return View(permisos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Permiso permiso)
        {
            await _permisoService.CrearPermiso(permiso);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var permiso = await _permisoService.ObtenerPermiso(id);

            return View(permiso);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Permiso permiso)
        {
            await _permisoService.EditarPermiso(permiso);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var permiso = await _permisoService.ObtenerPermiso(id);

            return View(permiso);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Permiso permiso)
        {
            await _permisoService.EliminarPermiso(permiso.IdPermiso);

            return RedirectToAction(nameof(Index));
        }
    }
}
