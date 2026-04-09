﻿using System.ComponentModel.DataAnnotations;

namespace InnovationLabBackend.Api.Models
{
    public class CoreValue
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Description { get; set; }

        public string? IconUrl { get; set; }

        public int Order { get; set; } = 0;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTimeOffset DeletedAt { get; set; }
    }
}
