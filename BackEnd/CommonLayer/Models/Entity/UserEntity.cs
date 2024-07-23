namespace CommonLayer.Models.Entity
{
    public class UserEntity
    {
        public Guid Id {  get; set; }
        public required string Name {  get; set; }

        public List<ToDoItemEntity>? ToDoItems { get; set; }
    }
}
