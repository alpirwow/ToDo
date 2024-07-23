using CommonLayer.Models.Entity;

namespace DataLayer.Interfaces
{
    public interface IUserRepository : ICrudGenericRepository<UserEntity, Guid>
    {
        public Task<bool> IsUserNameExist(string userName);
        public Task<bool> IsUserExist(Guid id);
        public Task<List<UserEntity>> GetAllFullAsync();
        public Task<UserEntity?> GetByIdFullAsync(Guid id);
    }
}
