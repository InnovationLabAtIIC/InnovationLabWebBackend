using InnovationLab.Auth.Dtos;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace InnovationLab.Auth.Models;

[AdaptTo(typeof(UserReadDto))]
[AdaptFrom(typeof(UserRegisterDto))]
[AdaptFrom(typeof(UserUpdateDto))]
public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string? Address { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = null;
    public DateTime? DeactivatedAt { get; set; } = null;
}