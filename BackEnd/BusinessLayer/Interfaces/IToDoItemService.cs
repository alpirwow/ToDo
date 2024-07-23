using CommonLayer.Models.Dto.ToDoItem;

namespace BusinessLayer.Interfaces
{
    public interface IToDoItemService
    {
        public Task AddAsync(ToDoItemPostDto dto);
        public Task UpdateAsync(ToDoItemPutDto dto);
        public Task DeleteByIdAsync(Guid id);
        public Task<List<ToDoItemGetDto>> GetAllAsync();
        public Task<ToDoItemGetDto?> GetByIdAsync(Guid id);
        public Task<List<ToDoItemGetDto>> GetWithConditionsAsync(bool? isGetCompleted, int? priorityLevel, Guid? userId);
        public Task<List<ToDoItemGetDto>> GetMyToDoWithConditionsAsync(bool? isGetCompleted, int? priorityLevel);
        public Task CompliteAsync(Guid id);
    }
}
