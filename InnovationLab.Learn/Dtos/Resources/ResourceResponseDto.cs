using System.ComponentModel.DataAnnotations;
using InnovationLab.Learn.Models;
using Mapster;

namespace InnovationLab.Learn.Dtos.Resources;

[AdaptFrom(typeof(Resource))]
public record ResourceResponseDto(
    Guid Id,
    [Required] string Title,
    [Required] string Link,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);