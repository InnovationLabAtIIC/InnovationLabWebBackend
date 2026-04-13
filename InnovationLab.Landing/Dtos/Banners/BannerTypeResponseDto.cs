using InnovationLab.Shared.Enums;

namespace InnovationLab.Landing.Dtos.Banners;

public record BannerTypeResponseDto
(
    bool Success,
    string? Url,
    MediaType MediaType,
    string? ErrorMessage
);
