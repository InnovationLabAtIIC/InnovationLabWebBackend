using System.ComponentModel.DataAnnotations;
using InnovationLab.Landing.Models;
using Mapster;

namespace InnovationLab.Landing.Dtos.EventRegistrations;

[AdaptTo(typeof(TeamMember))]
public record TeamMemberCreateDto
(
    [Required] string Name,
    [Required][EmailAddress] string Email,
    [Required][Phone] string Phone
);