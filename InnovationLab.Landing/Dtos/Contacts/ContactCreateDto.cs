using System.ComponentModel.DataAnnotations;
using InnovationLab.Landing.Models;
using Mapster;

namespace InnovationLab.Landing.Dtos.Contacts;

[AdaptTo(typeof(Contact))]
public record ContactCreateDto
(
    [Required][MaxLength(30)] string Name,
    [Required][EmailAddress] string Email,
    [Required][Phone] string PhoneNumber,
    [Required][MaxLength(50)] string Subject,
    [Required][MaxLength(500)] string Message
);
