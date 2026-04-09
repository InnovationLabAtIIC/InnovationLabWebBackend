using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InnovationLab.Shared.Models;

namespace InnovationLab.Landing.Models;

public class TeamMember : BaseModel
{
    [Required] public required Guid RegistrationId { get; set; }
    [ForeignKey(nameof(RegistrationId))] public EventRegistration? Registration { get; set; }
    [Required] public required string Name { get; set; }
    [Required][EmailAddress] public required string Email { get; set; }
    [Required][Phone] public required string Phone { get; set; }
}