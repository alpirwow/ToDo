using CommonLayer.Models.Dto.ToDoItem;
using CommonLayer.Models.Entity;

namespace CommonLayer.Models.Dto.User
{
    public class UserGetDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<ToDoItemGetDto>? ToDoItems { get; set; }

        public UserGetDto(UserEntity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            ToDoItems = entity.ToDoItems?.Select(x => new ToDoItemGetDto(x)).OrderBy(x => x.DueDate);
        }
    }
}
