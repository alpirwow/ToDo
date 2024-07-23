using System.ComponentModel.DataAnnotations;

namespace CommonLayer.Models.Dto.Priority
{
    public class PriorityDto
    {
        [Required(ErrorMessage = "Not set priority level")]
        [Range(1, int.MaxValue, ErrorMessage = "Priority level must be a positive number.")]
        public required int Level { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Priority level must be a positive number.")]
        public int? OldLevel { get; set; }

        public int? CountUsed { get; set; }
    }
}
