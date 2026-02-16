using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CapaDato
{
    public interface IRepository<T> where T : class
    {
        //busca por Id 
        Task<T> GetByIdAsync(int id);
        //Todos los registro
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
}
