using CommonLayer.Models.Entity;
using DataLayer.Contexts;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataLayer.Repositories
{
    internal class CrudGenericRepository<T, TId> : ICrudGenericRepository<T, TId> where T : class
    {
        private protected readonly PostgreSQLContext _context;
        private protected readonly DbSet<T> _dbSet;
        private readonly ILogger<CrudGenericRepository<T, TId>> _logger;

        public CrudGenericRepository(PostgreSQLContext context, ILogger<CrudGenericRepository<T, TId>> logger)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            _logger = logger;
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            _logger.LogInformation("Getting all entities of type {EntityType}", typeof(T).Name);

            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(TId id)
        {
            _logger.LogInformation("Getting entity of type {EntityType} with ID {Id}", typeof(T).Name, id);

            var entity = await _dbSet.FindAsync(id);

            if (entity == null)
                _logger.LogWarning("Entity of type {EntityType} with ID {Id} not found", typeof(T).Name, id);

            return entity;
        }

        public virtual async Task AddAsync(T entity)
        {
            _logger.LogInformation("Adding new entity of type {EntityType}", typeof(T).Name);
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Entity of type {EntityType} added successfully", typeof(T).Name);
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _logger.LogInformation("Updating entity of type {EntityType}", typeof(T).Name);

            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Entity of type {EntityType} updated successfully", typeof(T).Name);
        }

        public virtual async Task DeleteAsync(TId id)
        {
            _logger.LogInformation("Deleting entity of type {EntityType} with ID {Id}", typeof(T).Name, id);

            var entity = await GetByIdAsync(id);

            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Entity of type {EntityType} with ID {Id} deleted successfully", typeof(T).Name, id);
            }
            else
                _logger.LogWarning("Entity of type {EntityType} with ID {Id} not found, deletion skipped", typeof(T).Name, id);

        }
    }
}
