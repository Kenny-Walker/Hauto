using Hauto.Context;
using Hauto.Interface.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hauto.Implementations.Repository
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class, new()
    {
        protected HautoContext _Context;
        public async Task<T> CreateAsync(T entity)
        {
            await _Context.Set<T>().AddAsync(entity);
            await _Context.SaveChangesAsync();
            return entity;
        }
        public async Task<T> UpdateAsync(T entity)
        {
            _Context.Set<T>().Update(entity);
            await _Context.SaveChangesAsync();
            return entity;
        }
        public async Task<T> GetAsync(int id)
        {
            return await _Context.Set<T>().FindAsync(id);
        }
        public async Task<T> GetAsync(Expression<Func<T, bool>> expression)
        {
            return await _Context.Set<T>().FirstOrDefaultAsync(expression);
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _Context.Set<T>().ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression)
        {
            return await _Context.Set<T>().Where(expression).ToListAsync();
        }
        public async Task<bool> DeleteAsync(T entity)
        {
            _Context.Set<T>().Remove(entity);
            await _Context.SaveChangesAsync();
            return true;
        }
    }
}



