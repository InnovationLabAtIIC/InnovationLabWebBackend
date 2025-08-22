using System.ComponentModel.DataAnnotations;

namespace InnovationLab.Auth.Dtos;

public record LoginRequestDto(
    [Required][EmailAddress] string Email,
    [Required] string Password
);