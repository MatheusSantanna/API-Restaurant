using backend.Data;
using backend.Repositories;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.EntityFrameworkCore;


namespace backend.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }


        public async Task AddAsync(T entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
        }


        public IQueryable<T> GetAllAsync()
        {
            return  _dbSet.AsNoTracking();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
       

        public void  Update(T entity)
        {
             _dbSet.Update(entity);
            _context.SaveChanges();
        }

        public async Task DeleteAsync(int id) 
        {
            var entity = await _dbSet.FindAsync(id);

            if(entity != null)
            {
                _dbSet.Remove(entity);
            }
        }
    }
}
