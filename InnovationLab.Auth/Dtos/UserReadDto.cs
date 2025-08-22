using System.ComponentModel.DataAnnotations;

namespace InnovationLab.Auth.Dtos;

public record UserReadDto
(
    Guid Id,
    string UserName,
    string FirstName,
    string LastName,
    [EmailAddress] string Email,
    [Phone] string PhoneNumber,
    DateTime DateOfBirth,
    bool EmailConfirmed,
    bool TwoFactorEnabled,
    string? Address,
    string? ProfilePictureUrl,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? DeactivatedAt
);