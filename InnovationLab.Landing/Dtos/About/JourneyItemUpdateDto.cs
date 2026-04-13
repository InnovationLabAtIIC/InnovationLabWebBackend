﻿using InnovationLab.Landing.Models;
using Mapster;

namespace InnovationLab.Landing.Dtos.About;

[AdaptTo(typeof(JourneyItem))]
public record JourneyItemUpdateDto
(
    string? Title,
    string? Description,
    IFormFile? Image,
    DateTimeOffset? Date,
    int? Order
);
