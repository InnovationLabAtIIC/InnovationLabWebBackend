using System.ComponentModel.DataAnnotations;
using InnovationLab.Landing.Enums;
using InnovationLab.Landing.Models;
using Mapster;

namespace InnovationLab.Landing.Dtos.Banners;

[AdaptTo(typeof(Banner))]
public record BannerUpdateDto
(
    [Url] string? Url,
    MediaType? Type,
    string? Title,
    string? SubTitle,
    string? Caption,
    DateTimeOffset? ScheduledStart,
    DateTimeOffset? ScheduledEnd
);
