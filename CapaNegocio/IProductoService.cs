using System;
using System.Collections.Generic;
using System.Text;
using CapaNegocio.DTOs;

namespace CapaNegocio
{
    public interface IProductoService
    {
        Task<IEnumerable<ProductoDTO>> GetAllProductosAsync();
        Task<ProductoDTO> GetProductosByIdAsync(int id);
    }
}
