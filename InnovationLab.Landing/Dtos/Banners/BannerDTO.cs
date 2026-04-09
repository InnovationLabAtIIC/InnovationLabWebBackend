using AutoMapper;
using InnovationLabBackend.Api.Enums;
using System.ComponentModel.DataAnnotations;

namespace InnovationLabBackend.Api.Dtos.Banners
{
    [AutoMap(typeof(Models.Banner), ReverseMap = true)]
    public class BannerDTO
    {
        public Guid Id { get; set; }
        [Required]
        public required string Url { get; set; }
        [Required]
        public MediaType Type { get; set; }
        [Required]
        public required string Title { get; set; }
        public required string SubTitle { get; set; }
        public required string Caption { get; set; }
        public bool Current { get; set; }
        public int Version { get; set; }
        public string? ParentId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ScheduledStart { get; set; }
        public DateTimeOffset? ScheduledEnd { get; set; }
    }
}
