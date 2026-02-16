using CapaNegocio;
using Microsoft.AspNetCore.Mvc;
using SistemaUsuario.CapaNegocio;

namespace SistemaUsuarios.Web.Controllers
{
    public class ProductoController : Controller
    {

        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }
        public IActionResult Index()
        {
            return View();
        }
        //Anotaciones
        [HttpGet]
        async Task<IActionResult> Get()
        {
            try
            {
                var productos = await _productoService.GetAllProductosAsync();
                return Json(new { success = true, data = productos });
            }
            catch (Exception ex)
            {

                return Json(new { error = ex.Message });
            }

        }


        [HttpGet]
        async Task<IActionResult> GetById(int id) {
            try
            {
                var productos = await _productoService.GetProductosByIdAsync(id);
                return Json(new {success =  true,data = productos});
            }
            catch (Exception ex)
            {

                return Json(new { error = ex.Message });
;            }
        }

    }
}
