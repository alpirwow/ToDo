namespace CommonLayer.Models.Entity
{
    public class ToDoItemEntity
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateOnly DueDate { get; set; }

        public required int PriorityLevel { get; set; }
        public required Guid UserId { get; set; }

        public PriorityEntity? Priority { get; set; }
        public UserEntity? User { get; set; }
    }
}
