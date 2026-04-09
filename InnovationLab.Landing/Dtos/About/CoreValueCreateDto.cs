﻿using AutoMapper;
using InnovationLabBackend.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace InnovationLabBackend.Api.Dtos.About
{
    [AutoMap(typeof(CoreValue), ReverseMap = true)]
    public class CoreValueCreateDto
    {
        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Description { get; set; }

        public IFormFile? Icon { get; set; }

        public int Order { get; set; } = 0;
    }
}
