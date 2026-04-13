using System.ComponentModel.DataAnnotations;
using InnovationLab.Auth.Models;
using Mapster;

namespace InnovationLab.Auth.Dtos;

[AdaptFrom(typeof(User))]
public record UserResponseDto
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