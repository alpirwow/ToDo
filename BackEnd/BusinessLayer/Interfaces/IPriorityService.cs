using CommonLayer.Models.Dto.Priority;

namespace BusinessLayer.Interfaces
{
    public interface IPriorityService
    {
        public Task AddAsync(PriorityDto dto);
        public Task UpdateAsync(PriorityDto dto);
        public Task DeleteByIdAsync(PriorityDto dto);
        public Task<List<PriorityDto>> GetAllAsync();
    }
}
