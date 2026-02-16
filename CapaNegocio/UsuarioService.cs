using System;
using System.Collections.Generic;
using System.Text;
using CapaDato;
using CapaNegocio.DTOs;
using System.Threading.Tasks;
using System.Linq;

using CapaNegocio;
using Microsoft.EntityFrameworkCore;
using SistemaUsuario.CapaNegocio;
using SistemaUsuario.CapaDato;


namespace SistemaUsuario.CapaNegocio
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordService _passwordService;

        public UsuarioService(IUsuarioRepository usuarioRepository, IPasswordService passwordService)
        {
            _usuarioRepository = usuarioRepository;
            _passwordService = passwordService;
        }

        public async Task<IEnumerable<UsuarioDTO>> GetAllUsuariosAsync()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            var listaUsuarioDTO = usuarios.Select(u => MapToDTO(u)).ToList();
            return listaUsuarioDTO;
        }

        public async Task<UsuarioDTO> GetUsuarioByIdAsync(int id)
        {
         //capadata.usuario
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            return usuario != null ? MapToDTO(usuario) : null;
        }

        public async Task<UsuarioDTO> CreateUsuarioAsync(UsuarioDTO usuarioDto)
        {
            try
            {
                //  AGREGAR ESTA VALIDACIÓN PRIMERO
                if (string.IsNullOrWhiteSpace(usuarioDto.Password))
                {
                    throw new Exception("La contraseña es requerida al crear un usuario");
                }

                // Validar que no exista el nombre de usuario
                if (await _usuarioRepository.NombreUsuarioExisteAsync(usuarioDto.NombreUsuario))
                {
                    throw new Exception("El nombre de usuario ya existe");
                }

                // Validar que no exista el correo
                if (await _usuarioRepository.CorreoExisteAsync(usuarioDto.Correo))
                {
                    throw new Exception("El correo electrónico ya está registrado");
                }

                var usuario = new Usuario
                {
                    NombreCompleto = usuarioDto.NombreCompleto,
                    NombreUsuario = usuarioDto.NombreUsuario,
                    Password = _passwordService.HashPassword(usuarioDto.Password),
                    Correo = usuarioDto.Correo,
                    Estatus = usuarioDto.Estatus,
                    FechaAlta = DateTime.Now,
                    FechaModificacion = DateTime.Now
                };

                var result = await _usuarioRepository.AddAsync(usuario);
                return MapToDTO(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UsuarioDTO> UpdateUsuarioAsync(UsuarioDTO usuarioDto)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioDto.Id);
            if (usuario == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            // Validar que no exista el nombre de usuario (excluyendo el usuario actual)
            if (await _usuarioRepository.NombreUsuarioExisteAsync(usuarioDto.NombreUsuario, usuarioDto.Id))
            {
                throw new Exception("El nombre de usuario ya existe");
            }

            // Validar que no exista el correo (excluyendo el usuario actual)
            if (await _usuarioRepository.CorreoExisteAsync(usuarioDto.Correo, usuarioDto.Id))
            {
                throw new Exception("El correo electrónico ya está registrado");
            }

            usuario.NombreCompleto = usuarioDto.NombreCompleto;
            usuario.NombreUsuario = usuarioDto.NombreUsuario;
            usuario.Correo = usuarioDto.Correo;
            usuario.Estatus = usuarioDto.Estatus;
            usuario.FechaModificacion = DateTime.Now;

            // Solo actualizar la contraseña si se proporciona una nueva
            if (!string.IsNullOrEmpty(usuarioDto.Password))
            {
                usuario.Password = _passwordService.HashPassword(usuarioDto.Password);
            }

            var result = await _usuarioRepository.UpdateAsync(usuario);
            return MapToDTO(result);
        }

        public async Task<bool> DeleteUsuarioAsync(int id)
        {
            var usuarioExiste = _usuarioRepository.GetByIdAsync(id);
            if (usuarioExiste != null)
            {
                usuarioExiste.Result.Estatus = false;
            }
            return await _usuarioRepository.DeleteAsync(usuarioExiste.Result);
        }

        public async Task<UsuarioDTO> LoginAsync(LoginDTO loginDto)
        {
            var usuario = await _usuarioRepository.GetByNombreUsuarioAsync(loginDto.NombreUsuario);

            if (usuario == null)
            {
                return null;
            }

            if (!_passwordService.VerifyPassword(loginDto.Password, usuario.Password))
            {
                return null;
            }

            if (!usuario.Estatus)
            {
                throw new Exception("Usuario inactivo. Contacte al administrador.");
            }

            return MapToDTO(usuario);
        }

        public async Task<UsuarioDTO> RegistroAsync(RegistroDTO registroDto)
        {
            // Validar que no exista el nombre de usuario
            if (await _usuarioRepository.NombreUsuarioExisteAsync(registroDto.NombreUsuario))
            {
                throw new Exception("El nombre de usuario ya existe");
            }

            // Validar que no exista el correo
            if (await _usuarioRepository.CorreoExisteAsync(registroDto.Correo))
            {
                throw new Exception("El correo electrónico ya está registrado");
            }

            var usuario = new Usuario
            {
                NombreCompleto = registroDto.NombreCompleto,
                NombreUsuario = registroDto.NombreUsuario,
                Password = _passwordService.HashPassword(registroDto.Password),
                Correo = registroDto.Correo,
                Estatus = true, // Activo por defecto
                FechaAlta = DateTime.Now
            };

            var result = await _usuarioRepository.AddAsync(usuario);
            return MapToDTO(result);
        }

        public async Task<bool> NombreUsuarioExisteAsync(string nombreUsuario, int? excludeId = null)
        {
            return await _usuarioRepository.NombreUsuarioExisteAsync(nombreUsuario, excludeId);
        }

        public async Task<bool> CorreoExisteAsync(string correo, int? excludeId = null)
        {
            return await _usuarioRepository.CorreoExisteAsync(correo, excludeId);
        }

        public async Task<DashboardDTO> GetDashboardDataAsync()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            var usuariosList = usuarios.ToList();

            var hoy = DateTime.Today;
            var inicioSemana = hoy.AddDays(-(int)hoy.DayOfWeek);
            var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);

            var dashboard = new DashboardDTO
            {
                TotalUsuarios = usuariosList.Count,
                UsuariosActivos = usuariosList.Count(u => u.Estatus),
                UsuariosInactivos = usuariosList.Count(u => !u.Estatus),
                UsuariosRegistradosHoy = usuariosList.Count(u => u.FechaAlta.Date == hoy),
                UsuariosRegistradosSemana = usuariosList.Count(u => u.FechaAlta >= inicioSemana),
                UsuariosRegistradosMes = usuariosList.Count(u => u.FechaAlta >= inicioMes),
                RegistrosPorDia = usuariosList
                    .GroupBy(u => u.FechaAlta.Date)
                    .OrderBy(g => g.Key)
                    .Select(g => new RegistrosPorDiaDTO
                    {
                        Fecha = g.Key.ToString("dd/MM/yyyy"),
                        Cantidad = g.Count()
                    })
                    .ToList()
            };
            
            return dashboard;
        }

        private UsuarioDTO MapToDTO(Usuario usuario)
        {
            return new UsuarioDTO
            {
                Id = usuario.Id,
                NombreCompleto = usuario.NombreCompleto,
                NombreUsuario = usuario.NombreUsuario,
                Correo = usuario.Correo,
                Estatus = usuario.Estatus,
                FechaAlta = usuario.FechaAlta,
                FechaModificacion = usuario.FechaModificacion,
               // Password = usuario.Password
            };
        }
    }
}
