using System.ComponentModel.DataAnnotations;

namespace CommonLayer.Models.Dto.User
{
    public class UserPutDto
    {
        [Required(ErrorMessage = "Not set user id")]
        public required Guid Id { get; set; }

        [Required(ErrorMessage = "Not set user name")]
        [StringLength(50, ErrorMessage = "User name length can't be more than 50 characters.")]
        public required string Name { get; set; }
    }
}
