using CommonLayer.Models.Dto.Priority;
using CommonLayer.Models.Entity;

namespace DataLayer.Interfaces
{
    public interface IPriorityRepository : ICrudGenericRepository<PriorityEntity, int>
    {
        public Task<bool> IsPriorityLevelExist(int level);
        public Task<List<PriorityDto>> GetAllWithStaticticAsync();
    }
}
