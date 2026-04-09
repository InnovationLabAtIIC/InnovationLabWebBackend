using System.ComponentModel.DataAnnotations;
using InnovationLab.Shared.Models;

namespace InnovationLab.Landing.Models;

public class Contact : BaseModel
{
    [Required][MaxLength(30)] public required string Name { get; set; }
    [Required][EmailAddress] public required string Email { get; set; }
    [Required][Phone] public required string PhoneNumber { get; set; }
    [Required][MaxLength(50)] public required string Subject { get; set; }
    [Required][MaxLength(500)] public required string Message { get; set; }
}
