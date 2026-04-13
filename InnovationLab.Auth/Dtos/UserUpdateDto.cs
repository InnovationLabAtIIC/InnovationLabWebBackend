using System.ComponentModel.DataAnnotations;
using InnovationLab.Auth.Models;
using Mapster;

namespace InnovationLab.Auth.Dtos;

[AdaptTo(typeof(User))]
public record UserUpdateDto
(
    string FirstName,
    string LastName,
    [Phone] string PhoneNumber,
    DateTime DateOfBirth,
    string? Address,
    string? ProfilePictureUrl
);