using System.ComponentModel.DataAnnotations;
using InnovationLab.Auth.Models;
using Mapster;

namespace InnovationLab.Auth.Dtos;

[AdaptTo(typeof(User))]
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