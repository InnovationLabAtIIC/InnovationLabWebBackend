using System.ComponentModel.DataAnnotations;
using Mapster;

namespace InnovationLab.Landing.Dtos.About;

[AdaptTo(typeof(Models.About))]
public record AboutCreateDto
(
    [Required] string Mission,
    [Required] string Vision,
    [Required] string ParentOrgName,
    [Required] string ParentOrgDescription,
    IFormFile? ParentOrgLogo,
    [Url] string? ParentOrgWebsiteUrl
);
