using CapaNegocio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SistemaUsuario.CapaNegocio;

namespace SistemaUsuarios.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public DashboardController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardData()
        {
            try
            {
                var data = await _usuarioService.GetDashboardDataAsync();
                return Json(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}