using CapaNegocio;
using CapaNegocio.DTOs;
using Microsoft.AspNetCore.Mvc;
using SistemaUsuario.CapaNegocio;

namespace SistemaUsuarios.Web.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public IActionResult Registrar()
        {
            return View();
        }

        // Método GET para cargar la vista de actualizar
        [HttpGet]
        public async Task<IActionResult> Actualizar(int id)
        {
            if (id == 0)
            {
                TempData["Error"] = "ID inválido";
                return RedirectToAction("Lista");
            }

            try
            {
                var usuario = await _usuarioService.GetUsuarioByIdAsync(id);
                if (usuario == null)
                {
                    TempData["Error"] = "Usuario no encontrado";
                    return RedirectToAction("Lista");
                }

                return View(usuario);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Lista");
            }
        }

        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            if (id == 0)
            {
                TempData["Error"] = "ID inválido";
                return RedirectToAction("Lista");
            }

            return View(id);
        }

        public IActionResult Lista()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var usuarios = await _usuarioService.GetAllUsuariosAsync();
                return Json(new { success = true, data = usuarios });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var usuario = await _usuarioService.GetUsuarioByIdAsync(id);
                if (usuario == null)
                {
                    return Json(new { success = false, message = "Usuario no encontrado" });
                }
                return Json(new { success = true, data = usuario });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] UsuarioDTO model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join(", ", errors) });
            }

            try
            {
                var usuario = await _usuarioService.CreateUsuarioAsync(model);
                return Json(new
                {
                    success = true,
                    message = "Usuario creado exitosamente",
                    data = usuario,
                    msj = "Yo voy a enviar esto a mi vista"
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // SOLO UN método Update - ESTE ES EL CORRECTO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromBody] UsuarioDTO model)
        {

            ModelState.Remove("Password");
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join(", ", errors) });
            }

            try
            {
                DateTime fechaActual = DateTime.Now;
                model.FechaModificacion = fechaActual;
                var usuario = await _usuarioService.UpdateUsuarioAsync(model);
                return Json(new { success = true, message = "Usuario actualizado exitosamente", data = usuario });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _usuarioService.DeleteUsuarioAsync(id);
                if (result)
                {
                    return Json(new { success = true, message = "Usuario eliminado exitosamente" });
                }
                return Json(new { success = false, message = "No se pudo eliminar el usuario" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ValidarNombreUsuario(string nombreUsuario, int? id)
        {
            var existe = await _usuarioService.NombreUsuarioExisteAsync(nombreUsuario, id);
            return Json(!existe);
        }

        [HttpGet]
        public async Task<IActionResult> ValidarCorreo(string correo, int? id)
        {
            var existe = await _usuarioService.CorreoExisteAsync(correo, id);
            return Json(!existe);
        }
    }
}