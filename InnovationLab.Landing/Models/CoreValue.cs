﻿using System.ComponentModel.DataAnnotations;
using InnovationLab.Shared.Models;

namespace InnovationLab.Landing.Models;

public class CoreValue : BaseModel
{
    [Required] public required string Title { get; set; }

    [Required] public required string Description { get; set; }

    [Url] public string? IconUrl { get; set; }

    public int Order { get; set; }
}
