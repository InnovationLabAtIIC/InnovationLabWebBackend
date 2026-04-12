using InnovationLab.Landing.Enums;

namespace InnovationLab.Landing.Dtos.Banners;

public record BannerTypeResponseDto
(
    bool Success,
    string? Url,
    MediaType MediaType,
    string? ErrorMessage
);
