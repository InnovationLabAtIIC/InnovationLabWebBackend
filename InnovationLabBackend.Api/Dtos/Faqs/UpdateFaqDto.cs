﻿using AutoMapper;
using InnovationLabBackend.Api.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace InnovationLabBackend.Api.Dtos.Faqs
{
    [AutoMap(typeof(Faq), ReverseMap = true)]
    public class UpdateFaqDto
    {
        [Required]
        public string Question { get; set; } = default!;

        [Required]
        public string Answer { get; set; } = default!;

        [Required]
        public Guid CategoryId { get; set; }
    }
}
