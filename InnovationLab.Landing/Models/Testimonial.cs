using System.ComponentModel.DataAnnotations;

namespace InnovationLabBackend.Api.Models
{
    public class Testimonial
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Text { get; set; }
        public string? Designation { get; set; }
        public string? Organization { get; set; }
        public string? ImageUrl { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTimeOffset DeletedAt { get; set; }
    }
}
