using System;
using System.Collections.Generic;
using System.Text;
using CapaDato;
using CapaNegocio.DTOs;
using SistemaUsuario.CapaDato;

namespace CapaNegocio
{
    public class ProductoService : IProductoService
    {

        private readonly IProductosRepository _productoRepositorio;
       

        public ProductoService(IProductosRepository productoRepositorio)
        {
            _productoRepositorio = productoRepositorio;
            
        }

        public async  Task<IEnumerable<ProductoDTO>> GetAllProductosAsync()
        {
            var producto = await _productoRepositorio.GetAllAsync();
                        
            var listaUsuarioDTO =  producto.Select(u => MapToDTO(u)).ToList();
            return listaUsuarioDTO;
        }
           

        public async Task<ProductoDTO> GetProductosByIdAsync(int id)
        {
            var producto = await _productoRepositorio.GetByIdAsync(id);
            return producto != null ? this.MapToDTO(producto) : null; ;
        }


        private ProductoDTO MapToDTO(Producto usuario)
        {
            return new ProductoDTO
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Precio = usuario.Precio,
              
            };
        }
    }
}
