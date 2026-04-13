using Mapster;
using InnovationLab.Landing.Models;

namespace InnovationLab.Landing.Dtos.Categories;

[AdaptFrom(typeof(Category))]
public record CategoryResponseDto
(
    Guid Id,
    string Name,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
);
