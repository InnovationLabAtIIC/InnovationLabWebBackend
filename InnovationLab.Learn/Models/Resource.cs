using System.ComponentModel.DataAnnotations;
using InnovationLab.Learn.Dtos.Resources;
using InnovationLab.Shared.Models;
using Mapster;

namespace InnovationLab.Learn.Models;

[AdaptTo(typeof(ResourceReadDto))]
[AdaptFrom(typeof(ResourceCreateDto))]
public class Resource : BaseModel
{
    public required string Title { get; set; }
    public required string Link { get; set; }
    [Range(0.0, 5.0)] public float Ratings { get; set; } = 0.0F;
}