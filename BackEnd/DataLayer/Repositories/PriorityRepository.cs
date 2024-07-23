using CommonLayer.Models.Dto.Priority;
using CommonLayer.Models.Entity;
using DataLayer.Contexts;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataLayer.Repositories
{
    internal class PriorityRepository : CrudGenericRepository<PriorityEntity, int>, IPriorityRepository
    {
        private readonly ILogger<PriorityRepository> _logger;

        public PriorityRepository(PostgreSQLContext context, ILogger<PriorityRepository> logger) : base(context, logger)
        {
            _logger = logger;
        }

        public async Task<bool> IsPriorityLevelExist(int level)
        {
            _logger.LogInformation("Checking if priority level {Level} exists", level);

            var exists = await _dbSet.AsNoTracking().AnyAsync(x => x.Level == level);

            if (exists)
                _logger.LogInformation("Priority level {Level} exists", level);
            else
                _logger.LogWarning("Priority level {Level} does not exist", level);

            return exists;
        }

        public Task<List<PriorityDto>> GetAllWithStaticticAsync()
        {
            _logger.LogInformation("Getting all entities of type PriorityEntity");

            return _dbSet
                .Include(x => x.ToDoItems)
                .AsNoTracking()
                .Select(x => new PriorityDto()
                {
                    Level = x.Level,
                    CountUsed = x.ToDoItems != null ? x.ToDoItems.Count : 0
                })
                .ToListAsync();
        }

        public override async Task UpdateAsync(PriorityEntity newValue)
        {
            _logger.LogInformation("Starting update process for PriorityEntity with new level {NewLevel}", newValue.NewLevel);

            await _dbSet.AddAsync(new() { Level = newValue.NewLevel!.Value });
            _logger.LogInformation("Added new PriorityEntity with level {NewLevel}", newValue.NewLevel);

            var toDoList = await _context.ToDoList.Where(x => x.PriorityLevel == newValue.Level).ToListAsync();
            _logger.LogInformation("Fetched {Count} ToDoItems with old priority level {OldLevel}", toDoList.Count, newValue.Level);

            toDoList.ForEach(x =>
            {
                x.PriorityLevel = newValue.NewLevel!.Value;
                _logger.LogInformation("Updated ToDoItem with ID {Id} to new priority level {NewLevel}", x.Id, newValue.NewLevel);
            });

            var entity = await _dbSet.FirstOrDefaultAsync(x => x.Level == newValue.Level);
            if (entity is not null)
            {
                _dbSet.Remove(entity);
                _logger.LogInformation("Removed old PriorityEntity with level {OldLevel}", newValue.Level);
            }
            else            
                _logger.LogWarning("No PriorityEntity found with level {OldLevel} to remove", newValue.Level);
            

            await _context.SaveChangesAsync();
            _logger.LogInformation("Update process for PriorityEntity with new level {NewLevel} completed successfully", newValue.NewLevel);
        }
    }

}
