using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InnovationLabBackend.Api.Enums;

namespace InnovationLabBackend.Api.Models
{
    public class EventRegistration
    {
        [Key] public Guid Id { get; set; } = Guid.NewGuid();
        [Required] public required Guid EventId { get; set; }
        [ForeignKey(nameof(EventId))] public Event? Event { get; set; }
        [Required] public required EventRegistrationType Type { get; set; }
        [Required] public required string Name { get; set; }
        [Required][EmailAddress] public required string Email { get; set; }
        [Phone] public string? Phone { get; set; }
        public EventRegistrationStatus Status { get; set; } = EventRegistrationStatus.Pending;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; }
    }
}