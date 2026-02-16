using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio.DTOs
{
    public class DashboardDTO
    {
        public int TotalUsuarios { get; set; }
        public int UsuariosActivos { get; set; }
        public int UsuariosInactivos { get; set; }
        public int UsuariosRegistradosHoy { get; set; }
        public int UsuariosRegistradosSemana { get; set; }
        public int UsuariosRegistradosMes { get; set; }
        public List<RegistrosPorDiaDTO> RegistrosPorDia { get; set; }
    }
}
