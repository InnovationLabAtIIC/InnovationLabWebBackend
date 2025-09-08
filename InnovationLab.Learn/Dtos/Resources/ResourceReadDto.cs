using System.ComponentModel.DataAnnotations;

namespace InnovationLab.Learn.Dtos.Resources;

public record ResourceReadDto(
    Guid Id,
    [Required] string Title,
    [Required] string Link,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);