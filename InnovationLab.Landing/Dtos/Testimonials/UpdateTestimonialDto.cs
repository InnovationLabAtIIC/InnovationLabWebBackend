using AutoMapper;
using InnovationLabBackend.Api.Models;

namespace InnovationLabBackend.Api.Dtos.Testimonials
{
    [AutoMap(typeof(Testimonial), ReverseMap = true)]
    public class UpdateTestimonialDto
    {
        public string? Name { get; set; }
        public string? Text { get; set; }
        public string? Designation { get; set; }
        public string? Organization { get; set; }
        public IFormFile? ImageUrl { get; set; }
    }
}
