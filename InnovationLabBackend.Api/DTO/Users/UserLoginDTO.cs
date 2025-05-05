﻿using System.ComponentModel.DataAnnotations;

namespace InnovationLabBackend.Api.DTO.Users
{
    public class UserLoginDTO
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
