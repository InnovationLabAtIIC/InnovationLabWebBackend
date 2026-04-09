﻿using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace InnovationLabBackend.Api.Dtos.About
{
    [AutoMap(typeof(Models.About), ReverseMap = true)]
    public class ParentOrgUpdateDto
    {
        [Required]
        public required string ParentOrgName { get; set; }

        [Required]
        public required string ParentOrgDescription { get; set; }

        public IFormFile? ParentOrgLogo { get; set; }

        [Url]
        public string? ParentOrgWebsiteUrl { get; set; }
    }
}
