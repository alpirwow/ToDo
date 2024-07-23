using BusinessLayer.Interfaces;
using CommonLayer.Models.Dto.Priority;
using DataLayer.Interfaces;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Services
{
    internal class PriorityService : IPriorityService
    {
        private readonly IPriorityRepository _priorityRepository;
        private readonly ILogger<PriorityService> _logger;

        public PriorityService(IPriorityRepository priorityRepository, ILogger<PriorityService> logger)
        {
            _priorityRepository = priorityRepository;
            _logger = logger;
        }

        public async Task AddAsync(PriorityDto dto)
        {
            _logger.LogInformation("Attempting to add priority level: {Level}", dto.Level);

            var isExist = await _priorityRepository.IsPriorityLevelExist(dto.Level);

            if (isExist)
            {
                _logger.LogWarning("Priority level {Level} already exists.", dto.Level);
                return;
            }

            await _priorityRepository.AddAsync(new() { Level = dto.Level });
            _logger.LogInformation("Priority level {Level} added successfully.", dto.Level);
        }

        public async Task DeleteByIdAsync(PriorityDto dto)
        {
            _logger.LogInformation("Attempting to delete priority level: {Level}", dto.Level);
            await _priorityRepository.DeleteAsync(dto.Level);
            _logger.LogInformation("Priority level {Level} deleted successfully.", dto.Level);
        }

        public async Task<List<PriorityDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all priority levels.");
            var priorities = await _priorityRepository.GetAllWithStaticticAsync();
            _logger.LogInformation("Fetched {Count} priority levels.", priorities.Count);
            return priorities;
        }

        public async Task UpdateAsync(PriorityDto dto)
        {
            _logger.LogInformation("Attempting to update priority level: {Level}", dto.Level);

            var isExist = await _priorityRepository.IsPriorityLevelExist(dto.Level);
            if (isExist)
            {
                _logger.LogError("Priority level {Level} already taken.", dto.Level);
                throw new Exception($"This priority level '{dto.Level}' already taken.");
            }

            if (!dto.OldLevel.HasValue)
            {
                _logger.LogError("Old level not set for priority update.");
                throw new Exception("Not set old level.");
            }

            _logger.LogInformation("Updating priority from old level {OldLevel} to new level {NewLevel}.", dto.OldLevel, dto.Level);
            await _priorityRepository.UpdateAsync(new() { Level = dto.OldLevel!.Value, NewLevel = dto.Level });
            _logger.LogInformation("Priority level {Level} updated successfully.", dto.Level);
        }

    }
}
