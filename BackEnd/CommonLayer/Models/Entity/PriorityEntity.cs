using System.ComponentModel.DataAnnotations.Schema;

namespace CommonLayer.Models.Entity
{
    public class PriorityEntity
    {
        public required int Level { get; set; }
        [NotMapped]
        public int? NewLevel { get; set; }

        public List<ToDoItemEntity>? ToDoItems { get; set; }
    }
}
