using Mapster;
using InnovationLab.Landing.Models;

namespace InnovationLab.Landing.Dtos.Contacts;

[AdaptFrom(typeof(Contact))]
public record ContactResponseDto
(
    Guid Id,
    string Name,
    string Email,
    string PhoneNumber,
    string Subject,
    string Message,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
);
