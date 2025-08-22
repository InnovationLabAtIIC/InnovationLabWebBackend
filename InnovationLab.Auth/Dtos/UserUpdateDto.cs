using System.ComponentModel.DataAnnotations;

namespace InnovationLab.Auth.Dtos;

public record UserUpdateDto
(
    string FirstName,
    string LastName,
    [Phone] string PhoneNumber,
    DateTime DateOfBirth,
    string? Address,
    string? ProfilePictureUrl
);