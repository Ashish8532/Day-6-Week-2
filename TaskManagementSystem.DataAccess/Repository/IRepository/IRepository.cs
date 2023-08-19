using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T?> GetById(int id);
        Task<T> AddAsync(T entity);
        Task<T> Delete(T entity);
        Task<T?> GetFirstOrDefault(Expression<Func<T, bool>> filter);
    }
}
