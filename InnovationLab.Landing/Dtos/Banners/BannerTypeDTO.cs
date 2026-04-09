using InnovationLabBackend.Api.Enums;

namespace InnovationLabBackend.Api.Dtos.Banners
{
    public class BannerTypeDTO
    {
        public bool Success { get; set; }
        public string? Url { get; set; }
        public MediaType MediaType { get; set; }
        public string? ErrorMessage { get; set; }

    }
}
