using System;
using System.Collections.Generic;
using System.Text;
using SistemaUsuario.CapaDato;

namespace CapaDato
{
    public interface IProductosRepository
    {
        Task<Producto> GetByIdAsync(int id);
        Task<List<Producto>> GetAllAsync();
    }
}
