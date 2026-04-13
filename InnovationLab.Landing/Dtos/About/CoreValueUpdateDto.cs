﻿using InnovationLab.Landing.Models;
using Mapster;

namespace InnovationLab.Landing.Dtos.About;

[AdaptTo(typeof(CoreValue))]
public record CoreValueUpdateDto
(
    string? Title,
    string? Description,
    IFormFile? Icon,
    int? Order
);