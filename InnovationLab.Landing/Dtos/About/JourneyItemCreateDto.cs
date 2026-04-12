﻿using InnovationLab.Landing.Models;
using Mapster;
using System.ComponentModel.DataAnnotations;

namespace InnovationLab.Landing.Dtos.About;

[AdaptTo(typeof(JourneyItem))]
public record JourneyItemCreateDto
(
    [Required] string Title,
    [Required] string Description,
    IFormFile? Image,
    [Required] DateTimeOffset Date,
    int Order
);