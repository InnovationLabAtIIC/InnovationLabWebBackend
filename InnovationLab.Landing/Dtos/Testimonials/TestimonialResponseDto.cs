using InnovationLab.Landing.Models;
using Mapster;

namespace InnovationLab.Landing.Dtos.Testimonials;

[AdaptFrom(typeof(Testimonial))]
public record TestimonialResponseDto
(
    Guid Id,
    string Name,
    string Text,
    string? Designation,
    string? Organization,
    string? ImageUrl,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt,
    DateTimeOffset? DeletedAt
);
