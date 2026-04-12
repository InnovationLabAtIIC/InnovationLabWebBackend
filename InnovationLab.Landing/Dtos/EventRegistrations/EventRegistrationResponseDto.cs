using InnovationLab.Landing.Enums;
using InnovationLab.Landing.Models;
using Mapster;

namespace InnovationLab.Landing.Dtos.EventRegistrations;

[AdaptFrom(typeof(EventRegistration))]
public record EventRegistrationResponseDto
(
    Guid Id,
    Event? Event,
    EventRegistrationType Type,
    string Name,
    string Email,
    string? Phone,
    List<TeamMemberResponseDto>? TeamMembers,
    EventRegistrationStatus Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
);