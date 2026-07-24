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
            try
            {
                _dbSet.Add(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var erroOriginal = ex.InnerException?.Message;
                throw new Exception(erroOriginal, ex);
            }
            
        }


        public IQueryable<T> GetAllAsync()
        {
            try
            {
                return _dbSet.AsNoTracking();
            }
            catch (DbUpdateException ex)
            {
                var erroOriginal = ex.InnerException?.Message;
                throw new Exception(erroOriginal, ex);
            }
            
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (DbUpdateException ex)
            {
                var erroOriginal = ex.InnerException?.Message;
                throw new Exception(erroOriginal, ex);
            }
                
        }
       

        public void  Update(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                var erroOriginal = ex.InnerException?.Message;
                throw new Exception(erroOriginal, ex);
            }
        }

        public async Task DeleteAsync(int id) 
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);

                if(entity != null)
                {
                    _dbSet.Remove(entity);
                }    
            }
            catch (DbUpdateException ex)
            {
                var erroOriginal = ex.InnerException?.Message;
                throw new Exception(erroOriginal, ex);
            }
            
        }
    }
}
