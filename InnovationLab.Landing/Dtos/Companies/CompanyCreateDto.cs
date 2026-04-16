using System.ComponentModel.DataAnnotations;
using InnovationLab.Landing.Models;
using Mapster;

namespace InnovationLab.Landing.Dtos.Companies;

[AdaptTo(typeof(Company))]
public record CompanyCreateDto
(
    [Required] string Name,
    [Required] string Address,
    [EmailAddress] string? ContactEmail,
    [Required][Url] string LogoUrl,
    [Range(0, int.MaxValue)] int Priority
);
