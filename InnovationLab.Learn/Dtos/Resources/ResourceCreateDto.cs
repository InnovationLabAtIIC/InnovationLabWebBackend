using System.ComponentModel.DataAnnotations;

namespace InnovationLab.Learn.Dtos.Resources;

public record ResourceCreateDto(
    [Required] string Title,
    [Required] string Link,
    [Range(0.0, 5.0)] float Ratings = 0.0F
);