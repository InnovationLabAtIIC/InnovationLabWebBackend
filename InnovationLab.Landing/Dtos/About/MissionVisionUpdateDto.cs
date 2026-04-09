﻿using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace InnovationLabBackend.Api.Dtos.About
{
    [AutoMap(typeof(Models.About), ReverseMap = true)]
    public class MissionVisionUpdateDto
    {
        [Required]
        public required string Mission { get; set; }

        [Required]
        public required string Vision { get; set; }
    }
}
