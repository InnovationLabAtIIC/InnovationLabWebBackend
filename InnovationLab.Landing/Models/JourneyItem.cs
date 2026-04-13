﻿using System.ComponentModel.DataAnnotations;
using InnovationLab.Shared.Models;

namespace InnovationLab.Landing.Models;

public class JourneyItem : BaseModel
{
    [Required] public required string Title { get; set; }
    [Required] public required string Description { get; set; }
    public string? ImageUrl { get; set; }
    [Required] public DateTimeOffset Date { get; set; }
    public int Order { get; set; }
}
