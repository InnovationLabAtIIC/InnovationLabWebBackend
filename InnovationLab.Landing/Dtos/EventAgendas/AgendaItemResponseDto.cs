using InnovationLab.Landing.Models;
using Mapster;

namespace InnovationLab.Landing.Dtos.EventAgendas;

[AdaptFrom(typeof(AgendaItem))]
public record AgendaItemResponseDto
(
    Guid Id,
    TimeOnly StartTime,
    TimeOnly EndTime,
    string Title,
    string Description
);