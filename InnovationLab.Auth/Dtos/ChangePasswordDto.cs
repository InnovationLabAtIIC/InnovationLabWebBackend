using System.ComponentModel.DataAnnotations;

namespace InnovationLab.Auth.Dtos;

public record ChangePasswordDto(
    [Required] string CurrentPassword,
    [Required] string NewPassword
);