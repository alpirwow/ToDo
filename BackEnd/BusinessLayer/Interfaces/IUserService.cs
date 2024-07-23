using CommonLayer.Models.Dto.User;

namespace BusinessLayer.Interfaces
{
    public interface IUserService
    {
        public Task AddAsync(UserPostDto user);
        public Task UpdateAsync(UserPutDto user);
        public Task DeleteByIdAsync(Guid id);
        public Task<List<UserGetDto>> GetAllAsync();
        public Task<List<UserGetDto>> GetAllFullAsync();
        public Task<UserGetDto?> GetByIdAsync(Guid id);
        public Task<UserGetDto?> GetByIdFullAsync(Guid id);
    }
}
