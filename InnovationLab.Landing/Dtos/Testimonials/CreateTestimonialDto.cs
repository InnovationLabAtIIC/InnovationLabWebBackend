using System.ComponentModel.DataAnnotations;
using AutoMapper;
using InnovationLabBackend.Api.Models;

namespace InnovationLabBackend.Api.Dtos.Testimonials
{
    [AutoMap(typeof(Testimonial), ReverseMap = true)]
    public class CreateTestimonialDto
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Text { get; set; }
        public string? Designation { get; set; } 
        public string? Organization { get; set; } 
        public IFormFile? ImageUrl { get; set; } 
    }
}
