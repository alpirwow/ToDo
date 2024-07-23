using CommonLayer.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CommonLayer.Models.Dto.ToDoItem
{
    public class ToDoItemPostDto
    {
        [Required(ErrorMessage = "Not set title")]
        [StringLength(50, ErrorMessage = "Title length can't be more than 50 characters.")]
        public required string Title { get; set; }

        [StringLength(500, ErrorMessage = "Description length can't be more than 500 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Not set due date")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        [FutureOrTodayDate]
        public DateOnly DueDate { get; set; }

        [Required(ErrorMessage = "Not set priority level")]
        [Range(1, int.MaxValue, ErrorMessage = "Priority level must be a positive number.")]
        public required int PriorityLevel { get; set; }

        [Required(ErrorMessage = "Not set user id")]
        public required Guid UserId { get; set; }
    }
}
