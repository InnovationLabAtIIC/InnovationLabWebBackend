using System.ComponentModel.DataAnnotations;
using InnovationLab.Landing.Models;
using Mapster;

namespace InnovationLab.Landing.Dtos.Testimonials;

[AdaptTo(typeof(Testimonial))]
public record TestimonialUpdateDto
(
    [MinLength(1)] string? Name,
    [MinLength(1)] string? Text,
    string? Designation,
    string? Organization,
    IFormFile? ImageUrl
);
