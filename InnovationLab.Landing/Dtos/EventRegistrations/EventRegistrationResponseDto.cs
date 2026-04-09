using AutoMapper;
using InnovationLabBackend.Api.Enums;
using InnovationLabBackend.Api.Models;

namespace InnovationLabBackend.Api.Dtos.EventRegistrations
{
    [AutoMap(typeof(EventRegistration), ReverseMap = true)]
    public class EventRegistrationResponseDto
    {
        public Guid Id { get; set; }
        public Event? Event { get; set; }
        public required EventRegistrationType Type { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public List<TeamMemberResponseDto>? TeamMembers { get; set; }
        public EventRegistrationStatus Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}