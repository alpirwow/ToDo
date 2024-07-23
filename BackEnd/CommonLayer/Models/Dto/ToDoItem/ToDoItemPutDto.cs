using CommonLayer.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CommonLayer.Models.Dto.ToDoItem
{
    public class ToDoItemPutDto
    {
        [Required(ErrorMessage = "Not set to do item id")]
        public required Guid Id { get; set; }

        [StringLength(50, ErrorMessage = "Title length can't be more than 50 characters.")]
        public string? Title { get; set; }

        [StringLength(500, ErrorMessage = "Description length can't be more than 500 characters.")]
        public string? Description { get; set; }

        public bool? IsCompleted { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        [FutureOrTodayDate]
        public DateOnly? DueDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Priority level must be a positive number.")]
        public int? PriorityLevel { get; set; }

        [Required(ErrorMessage = "Not set user id")]
        public required Guid UserId { get; set; }
    }
}
