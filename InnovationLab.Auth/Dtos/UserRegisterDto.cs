using System.ComponentModel.DataAnnotations;

namespace InnovationLab.Auth.Dtos;

public record UserRegisterDto
(
    [Required] string UserName,
    [Required][MaxLength(50)] string FirstName,
    [Required][MaxLength(50)] string LastName,
    [Required][EmailAddress] string Email,
    string Password,
    [Phone] string PhoneNumber,
    DateTime DateOfBirth,
    string? Address,
    string? ProfilePictureUrl
);