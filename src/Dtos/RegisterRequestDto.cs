using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.src.Dtos
{
    public class RegisterRequestDto
    {
        [Required]
        public required string Password { get; set; }
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string FirstName { get; set; }
        [Required]
        public required string LastName { get; set; }

    }
}
