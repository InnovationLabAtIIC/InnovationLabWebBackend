using Mapster;
using InnovationLab.Landing.Enums;
using InnovationLab.Landing.Models;
using System.ComponentModel.DataAnnotations;

namespace InnovationLab.Landing.Dtos.Banners;

[AdaptTo(typeof(Banner))]
public record BannerCreateDto
(
    [Required] IFormFile Image,
    [Required] MediaType Type,
    [Required] string Title,
    [Required] string SubTitle,
    [Required] string Caption,
    bool Current,
    [Range(0, int.MaxValue)] int Version,
    string? ParentId,
    DateTimeOffset? ScheduledStart,
    DateTimeOffset? ScheduledEnd
);
