using AutoMapper;
using InnovationLabBackend.Api.Models;

namespace InnovationLabBackend.Api.Dtos.Faqs
{
    [AutoMap(typeof(Faq), ReverseMap = true)]
    public class FaqResponseDto
    {
        public Guid Id { get; set; }
        public string Question { get; set; } = default!;
        public string Answer { get; set; } = default!;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = default!;
    }
}
