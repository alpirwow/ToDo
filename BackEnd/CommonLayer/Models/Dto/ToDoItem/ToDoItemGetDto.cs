using CommonLayer.Models.Dto.User;
using CommonLayer.Models.Entity;

namespace CommonLayer.Models.Dto.ToDoItem
{
    public class ToDoItemGetDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateOnly DueDate { get; set; }

        public int PriorityLevel { get; set; }
        public Guid UserId { get; set; }

        public ToDoItemGetDto(ToDoItemEntity entity)
        {
            Id = entity.Id;
            Title = entity.Title;
            Description = entity.Description;
            IsCompleted = entity.IsCompleted;
            DueDate = entity.DueDate;
            PriorityLevel = entity.PriorityLevel;
            UserId = entity.UserId;
        }
    }
}
