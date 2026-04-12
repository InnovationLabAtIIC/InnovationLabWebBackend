using System.ComponentModel.DataAnnotations;
using InnovationLab.Landing.Models;
using Mapster;

namespace InnovationLab.Landing.Dtos.Testimonials;

[AdaptTo(typeof(Testimonial))]
public record TestimonialCreateDto
(
    [Required] string Name,
    [Required] string Text,
    string? Designation,
    string? Organization,
    IFormFile? ImageUrl
);
