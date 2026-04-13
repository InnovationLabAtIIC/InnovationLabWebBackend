using Mapster;
using InnovationLab.Shared.Enums;
using InnovationLab.Landing.Models;

namespace InnovationLab.Landing.Dtos.Banners;

[AdaptFrom(typeof(Banner))]
public record BannerResponseDto
(
    Guid Id,
    string Url,
    MediaType Type,
    string Title,
    string SubTitle,
    string Caption,
    bool Current,
    int Version,
    string? ParentId,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt,
    DateTimeOffset? ScheduledStart,
    DateTimeOffset? ScheduledEnd
);
