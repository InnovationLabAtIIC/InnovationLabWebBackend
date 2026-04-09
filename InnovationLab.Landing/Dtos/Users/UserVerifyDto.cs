using System.ComponentModel.DataAnnotations;

namespace InnovationLabBackend.Api.Dtos.Users
{
    public class UserVerifyDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(6)]
        public required string Otp { get; set; }
    }
}
