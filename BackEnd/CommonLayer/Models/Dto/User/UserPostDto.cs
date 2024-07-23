using System.ComponentModel.DataAnnotations;

namespace CommonLayer.Models.Dto.User
{
    public class UserPostDto
    {
        [Required(ErrorMessage = "Not set user name")]
        [StringLength(50, ErrorMessage = "User name length can't be more than 50 characters.")]
        public required string Name { get; set; }
    }
}
