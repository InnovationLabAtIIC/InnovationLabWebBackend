﻿using InnovationLab.Landing.Models;
using Mapster;

namespace InnovationLab.Landing.Dtos.About;

[AdaptFrom(typeof(CoreValue))]
public record CoreValueResponseDto
(
    Guid Id,
    string Title,
    string Description,
    string? IconUrl,
    int Order,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);