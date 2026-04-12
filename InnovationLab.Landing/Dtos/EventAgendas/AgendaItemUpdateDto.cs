using System.ComponentModel.DataAnnotations;
using InnovationLab.Landing.Models;
using Mapster;

namespace InnovationLab.Landing.Dtos.EventAgendas;

[AdaptTo(typeof(AgendaItem))]
public record AgendaItemUpdateDto
(
    [Required] TimeOnly StartTime,
    [Required] TimeOnly EndTime,
    [Required] string Title,
    [Required] string Description
);