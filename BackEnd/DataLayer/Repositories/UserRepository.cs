using CommonLayer.Models.Entity;
using DataLayer.Contexts;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataLayer.Repositories
{
    internal class UserRepository : CrudGenericRepository<UserEntity, Guid>, IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(PostgreSQLContext context, ILogger<UserRepository> logger) : base(context, logger)
        {
            _logger = logger;
        }

        public Task<bool> IsUserNameExist(string userName)
            => _dbSet
            .AsNoTracking()
            .AnyAsync(x => x.Name == userName);

        public Task<bool> IsUserExist(Guid id)
            => _dbSet
            .AsNoTracking()
            .AnyAsync(x => x.Id == id);

        public Task<List<UserEntity>> GetAllFullAsync()
            => _dbSet
            .Include(x => x.ToDoItems)
            .AsNoTracking()
            .ToListAsync();


        public Task<UserEntity?> GetByIdFullAsync(Guid id)
           => _dbSet
           .Include(x => x.ToDoItems)
           .AsNoTracking()
           .FirstOrDefaultAsync(x => x.Id == id);
    }
}
