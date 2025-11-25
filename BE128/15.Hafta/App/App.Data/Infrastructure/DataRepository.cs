using Microsoft.EntityFrameworkCore;

namespace App.Data.Infrastructure
{
    public interface IDataRepository
    {
        Task<T?> GetByIdAsync<T>(int id) where T : EntityBase;
        IQueryable<T> GetAll<T>() where T : EntityBase;
        Task<T> AddAsync<T>(T entity) where T : EntityBase;
        Task<T?> UpdateAsync<T>(T entity) where T : EntityBase;
        Task DeleteAsync<T>(int id) where T : EntityBase;
    }

    internal class DataRepository(DbContext dbContext) : IDataRepository
    {
        public async Task<T?> GetByIdAsync<T>(int id) where T : EntityBase
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        public IQueryable<T> GetAll<T>() where T : EntityBase
        {
            return dbContext.Set<T>();
        }

        public async Task<T> AddAsync<T>(T entity) where T : EntityBase
        {
            entity.Id = default;
            entity.CreatedAt = DateTime.UtcNow;

            await dbContext.Set<T>().AddAsync(entity);
            await dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<T?> UpdateAsync<T>(T entity) where T : EntityBase
        {
            if (entity.Id == default)
            {
                return null;
            }

            var dbEntity = await GetByIdAsync<T>(entity.Id);
            if (dbEntity == null)
            {
                return null;
            }

            entity.CreatedAt = dbEntity.CreatedAt;

            dbContext.Set<T>().Update(entity);
            await dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync<T>(int id) where T : EntityBase
        {
            var entity = await GetByIdAsync<T>(id);

            if (entity == null)
            {
                return;
            }

            dbContext.Set<T>().Remove(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}