using CommonLayer.Models.Dto.ToDoItem;
using CommonLayer.Models.Entity;

namespace DataLayer.Interfaces
{
    public interface IToDoItemRepository : ICrudGenericRepository<ToDoItemEntity, Guid>
    {
        public Task<List<ToDoItemGetDto>> GetWithConditionsAsync(bool? isGetCompleted, int? priorityLevel, Guid? userId);
        public Task CompliteAsync(Guid id);
    }
}
