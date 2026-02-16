using Microsoft.EntityFrameworkCore;
using SistemaUsuario.CapaDato;
using System.Linq;
using System.Threading.Tasks;

namespace CapaDato
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        //Creo el Constructor
        public UsuarioRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Usuario> GetByNombreUsuarioAsync(string nombreUsuario)
        {
            var usuarioConsultado = await _dbSet.FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);
            return usuarioConsultado;
        }

        public async Task<Usuario> GetByCorreoAsync(string correo)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Correo == correo);
        }

        public async Task<bool> NombreUsuarioExisteAsync(string nombreUsuario, int? excludeId = null)
        {
            if (excludeId.HasValue)
            {
                return await _dbSet.AnyAsync(u => u.NombreUsuario == nombreUsuario && u.Id != excludeId.Value);
            }
            return await _dbSet.AnyAsync(u => u.NombreUsuario == nombreUsuario);
        }

        public async Task<bool> CorreoExisteAsync(string correo, int? excludeId = null)
        {
            if (excludeId.HasValue)
            {
                return await _dbSet.AnyAsync(u => u.Correo == correo && u.Id != excludeId.Value);
            }
            return await _dbSet.AnyAsync(u => u.Correo == correo);
        }

        Task<List<Usuario>> IUsuarioRepository.GetAllAsync()
        {
            var listaUsuario = _dbSet.ToListAsync();
            return listaUsuario;
        }
    }
}
