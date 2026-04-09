using AutoMapper;
using InnovationLabBackend.Api.Models;

namespace InnovationLabBackend.Api.Dtos.Testimonials
{
    [AutoMap(typeof(Testimonial), ReverseMap = true)]
    public class TestimonialResponseDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Text { get; set; }
        public string? Designation { get; set; } 
        public string? Organization { get; set; } 
        public string? ImageUrl { get; set; } 
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset DeletedAt { get; set; }
    }
}
