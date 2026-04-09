namespace InnovationLabBackend.Api.Dtos.Banners
{
    public class BannerGetDTO
    {
        public Guid Id { get; set; }
        public string? Url { get; set; }
        public string? Type { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? Caption { get; set; }
        public bool Current { get; set; }
        public int Version { get; set; }
        public string? ParentId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ScheduledStart { get; set; }
        public DateTimeOffset? ScheduledEnd { get; set; }
    }
}
