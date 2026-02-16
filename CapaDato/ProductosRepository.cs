using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SistemaUsuario.CapaDato;

namespace CapaDato
{
    public class ProductosRepository : Repository<Producto>, IProductosRepository
    {
        public ProductosRepository(ApplicationDbContext context) : base(context)
        {

        }

          Task<List<Producto>> IProductosRepository.GetAllAsync()
        {
            return _dbSet.ToListAsync();
        }

        Task<Producto> IProductosRepository.GetByIdAsync(int id)
        {
            return _dbSet.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
