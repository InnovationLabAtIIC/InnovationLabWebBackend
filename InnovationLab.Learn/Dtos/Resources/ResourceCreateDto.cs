using System.ComponentModel.DataAnnotations;
using InnovationLab.Learn.Models;
using Mapster;

namespace InnovationLab.Learn.Dtos.Resources;

[AdaptTo(typeof(Resource))]
public record ResourceCreateDto(
    [Required] string Title,
    [Required] string Link,
    [Range(0.0, 5.0)] float Ratings = 0.0F
);