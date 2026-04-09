using AutoMapper;
using InnovationLabBackend.Api.Models;

namespace InnovationLabBackend.Api.Dtos.EventAgendas
{
    [AutoMap(typeof(EventAgenda), ReverseMap = true)]
    public class EventAgendaResponseDto
    {
        public Guid Id { get; set; }
        public required Guid EventId { get; set; }
        public required int Day { get; set; }
        public required List<AgendaItemResponseDto> Items { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}