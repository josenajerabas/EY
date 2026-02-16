using System;
using System.Collections.Generic;
using System.Text;
using CapaNegocio.DTOs;

namespace SistemaUsuario.CapaNegocio
{
    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioDTO>> GetAllUsuariosAsync();
        Task<UsuarioDTO> GetUsuarioByIdAsync(int id);
        Task<UsuarioDTO> CreateUsuarioAsync(UsuarioDTO usuarioDto);
        Task<UsuarioDTO> UpdateUsuarioAsync(UsuarioDTO usuarioDto);
        Task<bool> DeleteUsuarioAsync(int id);
        Task<UsuarioDTO> LoginAsync(LoginDTO loginDto);
        Task<UsuarioDTO> RegistroAsync(RegistroDTO registroDto);
        Task<bool> NombreUsuarioExisteAsync(string nombreUsuario, int? excludeId = null);
        Task<bool> CorreoExisteAsync(string correo, int? excludeId = null);
        Task<DashboardDTO> GetDashboardDataAsync();
    }
}
