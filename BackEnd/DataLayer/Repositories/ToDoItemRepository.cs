using CommonLayer.Models.Dto.ToDoItem;
using CommonLayer.Models.Entity;
using DataLayer.Contexts;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataLayer.Repositories
{
    internal class ToDoItemRepository : CrudGenericRepository<ToDoItemEntity, Guid>, IToDoItemRepository
    {
        private readonly ILogger<ToDoItemRepository> _logger;

        public ToDoItemRepository(PostgreSQLContext context, ILogger<ToDoItemRepository> logger) : base(context, logger)
        {
            _logger = logger;
        }

        public override async Task<List<ToDoItemEntity>> GetAllAsync()
        {
            _logger.LogInformation("Getting all ToDo items");

            var items = await _dbSet
                .OrderBy(x => x.DueDate)
                .AsNoTracking()
                .ToListAsync();

            _logger.LogInformation("{Count} ToDo items retrieved", items.Count);

            return items;
        }

        public override async Task<ToDoItemEntity?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Getting ToDo item with ID {Id}", id);

            var item = await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
                _logger.LogWarning("ToDo item with ID {Id} not found", id);
            else
                _logger.LogInformation("ToDo item with ID {Id} retrieved", id);

            return item;
        }

        public async Task<List<ToDoItemGetDto>> GetWithConditionsAsync(bool? isGetCompleted, int? priorityLevel, Guid? userId)
        {
            _logger.LogInformation("Getting ToDo items with conditions: isGetCompleted={IsGetCompleted}, priorityLevel={PriorityLevel}, , userId={UserId}", isGetCompleted, priorityLevel, userId);

            IQueryable<ToDoItemEntity> query = _dbSet.OrderBy(x => x.DueDate);

            if (isGetCompleted.HasValue)
                query = query.Where(x => x.IsCompleted == isGetCompleted.Value);

            if (priorityLevel.HasValue)
                query = query.Where(x => x.PriorityLevel == priorityLevel.Value);

            if (userId.HasValue)
                query = query.Where(x => x.UserId == userId.Value);

            var items = await query.Select(x => new ToDoItemGetDto(x))
                .AsNoTracking()
                .ToListAsync();

            _logger.LogInformation("{Count} ToDo items retrieved with conditions", items.Count);

            return items;
        }

        public async Task CompliteAsync(Guid id)
        {
            var toDo = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);

            if (toDo is null)
            {
                _logger.LogWarning("ToDo item with ID {Id} not found", id);
                return;
            }

            toDo.IsCompleted = !toDo.IsCompleted;

            await _context.SaveChangesAsync();

            _logger.LogInformation("ToDo item with ID {Id} updated successfully", id);
        }
    }

}
