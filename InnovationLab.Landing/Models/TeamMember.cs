using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InnovationLabBackend.Api.Models
{
    public class TeamMember
    {
        [Key] public Guid Id { get; set; } = Guid.NewGuid();
        [Required] public required Guid RegistrationId { get; set; }
        [ForeignKey(nameof(RegistrationId))] public EventRegistration? Registration { get; set; }
        [Required] public required string Name { get; set; }
        [Required][EmailAddress] public required string Email { get; set; }
        [Phone] public required string Phone { get; set; }
    }
}