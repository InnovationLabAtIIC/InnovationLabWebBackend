using InnovationLab.Shared.Enums;

namespace InnovationLab.Shared.Extensions;

public static class MediaTypeExtensions
{
    public static MediaType ToMediaType(this string contentType) =>
        contentType.ToLowerInvariant() switch
        {
            var ct when ct.StartsWith("image") => MediaType.Image,
            var ct when ct.StartsWith("video") => MediaType.Video,
            var ct when ct.Contains("pdf") => MediaType.Pdf,
            _ => MediaType.NotSupported
        };
}