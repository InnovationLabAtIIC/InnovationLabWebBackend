﻿using AutoMapper;
using InnovationLabBackend.Api.Models;

namespace InnovationLabBackend.Api.Dtos.About
{
    [AutoMap(typeof(CoreValue), ReverseMap = true)]
    public class CoreValueUpdateDto
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public IFormFile? Icon { get; set; }

        public int? Order { get; set; }
    }
}
