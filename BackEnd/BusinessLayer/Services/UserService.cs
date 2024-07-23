using BusinessLayer.Interfaces;
using CommonLayer.Models.Dto.User;
using DataLayer.Interfaces;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Services
{
    internal class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task AddAsync(UserPostDto user)
        {
            _logger.LogInformation("Attempting to add user with name: {Name}", user.Name);

            var isExist = await _userRepository.IsUserNameExist(user.Name);

            if (isExist)
            {
                _logger.LogError("User with name {Name} already exists.", user.Name);
                throw new Exception($"User with this name '{user.Name}' is exist.");
            }

            await _userRepository.AddAsync(new() { Name = user.Name });
            _logger.LogInformation("User with name {Name} added successfully.", user.Name);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            _logger.LogInformation("Attempting to delete user with id: {Id}", id);
            await _userRepository.DeleteAsync(id);
            _logger.LogInformation("User with id {Id} deleted successfully.", id);
        }

        public async Task<List<UserGetDto>> GetAllFullAsync()
        {
            _logger.LogInformation("Fetching all users with full details.");
            var users = await _userRepository.GetAllFullAsync();
            _logger.LogInformation("Fetched {Count} users with full details.", users.Count);
            return users.Select(x => new UserGetDto(x)).ToList();
        }

        public async Task<List<UserGetDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all users.");
            var users = await _userRepository.GetAllAsync();
            _logger.LogInformation("Fetched {Count} users.", users.Count);
            return users.Select(x => new UserGetDto(x)).ToList();
        }

        public async Task<UserGetDto?> GetByIdFullAsync(Guid id)
        {
            _logger.LogInformation("Fetching user with id: {Id} and full details.", id);
            var user = await _userRepository.GetByIdFullAsync(id);
            if (user is not null)
            {
                _logger.LogInformation("User with id {Id} found.", id);
                return new UserGetDto(user);
            }
            else
            {
                _logger.LogWarning("User with id {Id} not found.", id);
                return null;
            }
        }

        public async Task<UserGetDto?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching user with id: {Id}.", id);
            var user = await _userRepository.GetByIdAsync(id);
            if (user is not null)
            {
                _logger.LogInformation("User with id {Id} found.", id);
                return new UserGetDto(user);
            }
            else
            {
                _logger.LogWarning("User with id {Id} not found.", id);
                return null;
            }
        }

        public async Task UpdateAsync(UserPutDto dto)
        {
            _logger.LogInformation("Attempting to update user with id: {Id}", dto.Id);

            var user = await GetByIdFullAsync(dto.Id);

            if (user is not null && user.Name != dto.Name)
            {
                var isUserNameTaken = await _userRepository.IsUserNameExist(dto.Name);

                if (isUserNameTaken)
                {
                    _logger.LogError("Username {Name} is already taken.", dto.Name);
                    throw new Exception($"This username '{dto.Name}' already taken.");
                }

                await _userRepository.UpdateAsync(new() { Id = dto.Id, Name = dto.Name });
                _logger.LogInformation("User with id {Id} updated successfully.", dto.Id);
            }
            else
            {
                _logger.LogWarning("User with id {Id} not found or name is the same.", dto.Id);
            }
        }
    }

}
