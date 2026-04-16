using InnovationLab.Landing.Models;
using Mapster;

namespace InnovationLab.Landing.Dtos.Companies;

[AdaptFrom(typeof(Company))]
public record CompanyResponseDto
(
    Guid Id,
    string Name,
    string Address,
    string? ContactEmail,
    string LogoUrl,
    int Priority,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt,
    DateTimeOffset? DeletedAt
);
