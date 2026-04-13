using InnovationLab.Landing.Models;
using Mapster;

namespace InnovationLab.Landing.Dtos.EventRegistrations;

[AdaptFrom(typeof(TeamMember))]
public record TeamMemberResponseDto
(
    Guid Id,
    string Name,
    string Email,
    string Phone
);