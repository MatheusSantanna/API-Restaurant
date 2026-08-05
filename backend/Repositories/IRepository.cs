using System.Linq.Expressions;

namespace backend.Repositories
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void  Update(T entity);
        Task DeleteAsync(int id);
    }
}
