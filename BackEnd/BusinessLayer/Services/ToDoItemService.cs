using BusinessLayer.Interfaces;
using CommonLayer.Models.Dto.ToDoItem;
using DataLayer.Interfaces;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Services
{
    internal class ToDoItemService : IToDoItemService
    {
        private readonly IToDoItemRepository _toDoItemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPriorityRepository _priorityRepository;
        private readonly IIdentityUser _user;
        private readonly ILogger<ToDoItemService> _logger;

        public ToDoItemService(
            IToDoItemRepository toDoItemRepository,
            IUserRepository userRepository,
            IPriorityRepository priorityRepository,
            IIdentityUser identityUser,
            ILogger<ToDoItemService> logger)
        {
            _toDoItemRepository = toDoItemRepository;
            _userRepository = userRepository;
            _priorityRepository = priorityRepository;
            _user = identityUser;
            _logger = logger;
        }

        public async Task AddAsync(ToDoItemPostDto dto)
        {
            _logger.LogInformation("Attempting to add ToDo item with title: {Title}", dto.Title);

            var isUserExist = await _userRepository.IsUserExist(dto.UserId);

            if (!isUserExist)
            {
                _logger.LogError("User with id {UserId} not found.", dto.UserId);
                throw new Exception($"Can't find user with '{dto.UserId}' id");
            }

            var isPriorityLevelExist = await _priorityRepository.IsPriorityLevelExist(dto.PriorityLevel);

            if (!isPriorityLevelExist)
            {
                _logger.LogInformation("Priority level {PriorityLevel} does not exist. Adding new priority level.", dto.PriorityLevel);
                await _priorityRepository.AddAsync(new() { Level = dto.PriorityLevel });
            }

            await _toDoItemRepository.AddAsync(new()
            {
                Title = dto.Title,
                Description = dto.Description,
                IsCompleted = false,
                DueDate = dto.DueDate,
                PriorityLevel = dto.PriorityLevel,
                UserId = dto.UserId,
            });

            _logger.LogInformation("ToDo item with title {Title} added successfully.", dto.Title);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            _logger.LogInformation("Attempting to delete ToDo item with id: {Id}", id);
            await _toDoItemRepository.DeleteAsync(id);
            _logger.LogInformation("ToDo item with id {Id} deleted successfully.", id);
        }

        public async Task<List<ToDoItemGetDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all ToDo items.");
            var items = await _toDoItemRepository.GetAllAsync();
            _logger.LogInformation("Fetched {Count} ToDo items.", items.Count);
            return items.Select(x => new ToDoItemGetDto(x)).ToList();
        }

        public async Task<List<ToDoItemGetDto>> GetWithConditionsAsync(bool? isGetCompleted, int? priorityLevel, Guid? userId)
        {
            _logger.LogInformation("Fetching ToDo items with conditions: isGetCompleted={IsGetCompleted}, priorityLevel={PriorityLevel}, userId={UserId}", isGetCompleted, priorityLevel, userId);
            var items = await _toDoItemRepository.GetWithConditionsAsync(isGetCompleted, priorityLevel, userId);
            _logger.LogInformation("Fetched {Count} ToDo items with conditions.", items.Count);
            return items;
        }

        public async Task<List<ToDoItemGetDto>> GetMyToDoWithConditionsAsync(bool? isGetCompleted, int? priorityLevel)
        {
            if (_user.Id is null)
                return new();

            _logger.LogInformation("Fetching ToDo items with conditions: isGetCompleted={IsGetCompleted}, priorityLevel={PriorityLevel}, userId={UserId}", isGetCompleted, priorityLevel, _user.Id);
            var items = await _toDoItemRepository.GetWithConditionsAsync(isGetCompleted, priorityLevel, _user.Id);
            _logger.LogInformation("Fetched {Count} ToDo items with conditions.", items.Count);
            return items;
        }

        public async Task<ToDoItemGetDto?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching ToDo item with id: {Id}", id);
            var toDoItem = await _toDoItemRepository.GetByIdAsync(id);
            if (toDoItem is not null)
            {
                _logger.LogInformation("ToDo item with id {Id} found.", id);
                return new ToDoItemGetDto(toDoItem);
            }
            else
            {
                _logger.LogWarning("ToDo item with id {Id} not found.", id);
                return null;
            }
        }

        public async Task UpdateAsync(ToDoItemPutDto dto)
        {
            _logger.LogInformation("Attempting to update ToDo item with id: {Id}", dto.Id);

            var toDoItem = await _toDoItemRepository.GetByIdAsync(dto.Id);

            if (toDoItem is null)
            {
                _logger.LogError("Can't update ToDo item, because item with id {Id} not found.", dto.Id);
                throw new Exception($"Can't update to do item, because can't find with id - '{dto.Id}'");
            }

            var isUserExist = await _userRepository.IsUserExist(dto.UserId);

            if (!isUserExist)
            {
                _logger.LogError("User with id {UserId} not found.", dto.UserId);
                throw new Exception($"Can't find user with '{dto.UserId}' id");
            }

            if (dto.PriorityLevel is not null)
            {
                var isPriorityLevelExist = await _priorityRepository.IsPriorityLevelExist((int)dto.PriorityLevel);

                if (!isPriorityLevelExist)
                {
                    _logger.LogInformation("Priority level {PriorityLevel} does not exist. Adding new priority level.", dto.PriorityLevel);
                    await _priorityRepository.AddAsync(new() { Level = (int)dto.PriorityLevel });
                }
            }

            toDoItem.Title = dto.Title ?? toDoItem.Title;
            toDoItem.Description = dto.Description ?? toDoItem.Description;
            toDoItem.IsCompleted = dto.IsCompleted ?? toDoItem.IsCompleted;
            toDoItem.DueDate = dto.DueDate ?? toDoItem.DueDate;
            toDoItem.PriorityLevel = dto.PriorityLevel ?? toDoItem.PriorityLevel;
            toDoItem.UserId = dto.UserId;

            await _toDoItemRepository.UpdateAsync(toDoItem);
            _logger.LogInformation("ToDo item with id {Id} updated successfully.", dto.Id);
        }

        public Task CompliteAsync(Guid id)
            => _toDoItemRepository.CompliteAsync(id);
    }

}
