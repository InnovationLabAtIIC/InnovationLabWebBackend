using InnovationLabBackend.Api.Enums;

namespace InnovationLabBackend.Api.Dtos.Banners
{
    public class BannerUpdateDTO
    {

        public string? Url { get; set; }
        public MediaType? Type { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? Caption { get; set; }
        public DateTimeOffset? ScheduledStart { get; set; }
        public DateTimeOffset? ScheduledEnd { get; set; }
    }
}
