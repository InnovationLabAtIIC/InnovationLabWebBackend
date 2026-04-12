﻿using InnovationLab.Landing.Models;
using Mapster;

namespace InnovationLab.Landing.Dtos.About;

[AdaptFrom(typeof(JourneyItem))]
public record JourneyItemResponseDto
(
    Guid Id,
    string Title,
    string Description,
    string? ImageUrl,
    DateTimeOffset Date,
    int Order,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);