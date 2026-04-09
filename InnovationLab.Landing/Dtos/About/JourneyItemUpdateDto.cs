﻿using AutoMapper;
using InnovationLabBackend.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace InnovationLabBackend.Api.Dtos.About
{
    [AutoMap(typeof(JourneyItem), ReverseMap = true)]
    public class JourneyItemUpdateDto
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public IFormFile? Image { get; set; }

        public DateTimeOffset? Date { get; set; }

        public int? Order { get; set; }
    }
}
