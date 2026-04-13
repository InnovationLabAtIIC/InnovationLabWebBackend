using InnovationLab.Landing.Models;
using Mapster;

namespace InnovationLab.Landing.Dtos.Banners;

[AdaptTo(typeof(Banner))]
public record BannerScheduleUpdateDto
(
    DateTimeOffset? ScheduledStart,
    DateTimeOffset? ScheduledEnd
);
