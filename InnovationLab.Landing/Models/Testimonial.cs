using System.ComponentModel.DataAnnotations;
using InnovationLab.Shared.Models;

namespace InnovationLab.Landing.Models;

public class Testimonial : BaseModel
{
    [Required] public required string Name { get; set; }
    [Required] public required string Text { get; set; }
    public string? Designation { get; set; }
    public string? Organization { get; set; }
    [Url] public string? ImageUrl { get; set; }
}
