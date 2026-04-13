using System.ComponentModel.DataAnnotations;
using InnovationLab.Shared.Models;

namespace InnovationLab.Learn.Models;

public class Resource : BaseModel
{
    public required string Title { get; set; }
    public required string Link { get; set; }
    [Range(0.0, 5.0)] public float Ratings { get; set; } = 0.0F;
}