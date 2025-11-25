using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data
{
    public interface IDataRepository
    {
        // Temel CRUD işlemleri için bir altyapı hazırlanır.

        Task<IEnumerable<T>> GetAll<T>() where T : class; // GetAll<UserEntity>(); , GetAll<ProductEntity>();
        Task<T?> GetById<T>(int id) where T : class; // GetById<UserEntity>(int id)
        Task<T> Add<T>(T entity) where T : class;
        Task<T> Update<T>(T entity) where T : class;
        Task<T> Delete<T>(T entity) where T : class;

    }

    internal class DataRepository : IDataRepository
    {
        private readonly DbContext _dbContext;

        public DataRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<T>> GetAll<T>() where T : class
        {
           return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetById<T>(int id) where T : class
        {
           return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> Add<T>(T entity) where T : class
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> Update<T>(T entity) where T : class
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> Delete<T>(T entity) where T : class
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }  
    }
}
