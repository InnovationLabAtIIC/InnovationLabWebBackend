﻿using InnovationLab.Landing.Models;
using Mapster;
using System.ComponentModel.DataAnnotations;

namespace InnovationLab.Landing.Dtos.About;

[AdaptTo(typeof(CoreValue))]
public record CoreValueCreateDto
(
    [Required] string Title,
    [Required] string Description,
    IFormFile? Icon,
    int Order
);
