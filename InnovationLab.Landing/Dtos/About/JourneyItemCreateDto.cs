﻿using AutoMapper;
using InnovationLabBackend.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace InnovationLabBackend.Api.Dtos.About
{
    [AutoMap(typeof(JourneyItem), ReverseMap = true)]
    public class JourneyItemCreateDto
    {
        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Description { get; set; }

        public IFormFile? Image { get; set; }

        [Required]
        public DateTimeOffset Date { get; set; }

        public int Order { get; set; } = 0;
    }
}
