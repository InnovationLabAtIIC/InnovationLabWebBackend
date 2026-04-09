using InnovationLabBackend.Api.Enums;
using System.ComponentModel.DataAnnotations;

namespace InnovationLabBackend.Api.Models
{
    public class Banner
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public required string Url { get; set; }
        [Required]
        public MediaType Type { get; set; }
        [Required]
        public required string Title { get; set; }
        public required string SubTitle { get; set; }
        public required string Caption { get; set; }
        public bool Current { get; set; }
        public int Version { get; set; }
        public string? ParentId { get; set; } = null;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? ScheduledStart { get; set; } = null;
        public DateTimeOffset? ScheduledEnd { get; set; } = null;

    }
}
