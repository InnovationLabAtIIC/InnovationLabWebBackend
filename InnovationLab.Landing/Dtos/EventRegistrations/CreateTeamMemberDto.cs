using System.ComponentModel.DataAnnotations;
using AutoMapper;
using InnovationLabBackend.Api.Models;

namespace InnovationLabBackend.Api.Dtos.EventRegistrations
{
    [AutoMap(typeof(TeamMember), ReverseMap = true)]
    public class CreateTeamMemberDto
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Phone]
        public string? Phone { get; set; }
    }
}