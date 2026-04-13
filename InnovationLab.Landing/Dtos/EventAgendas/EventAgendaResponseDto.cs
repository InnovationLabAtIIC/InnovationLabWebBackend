using InnovationLab.Landing.Models;
using Mapster;

namespace InnovationLab.Landing.Dtos.EventAgendas;

[AdaptFrom(typeof(EventAgenda))]
public record EventAgendaResponseDto
(
    Guid Id,
    Guid EventId,
    int Day,
    List<AgendaItemResponseDto> Items,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
);