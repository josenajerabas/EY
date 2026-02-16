using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SistemaUsuario.CapaDato;

namespace CapaDato
{
    public interface IUsuarioRepository
    {
        Task<Usuario> GetByNombreUsuarioAsync(string nombreUsuario);
        Task<Usuario> GetByCorreoAsync(string correo);
        Task<bool> NombreUsuarioExisteAsync(string nombreUsuario, int? excludeId = null);
        Task<bool> CorreoExisteAsync(string correo, int? excludeId = null);

        Task<Usuario> GetByIdAsync(int id);
        Task<List<Usuario>> GetAllAsync();
        Task<Usuario> AddAsync(Usuario usuario);
        Task<Usuario> UpdateAsync(Usuario usuario);

        Task<bool>DeleteAsync(Usuario entidad);

    }
}
