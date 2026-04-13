using System.ComponentModel.DataAnnotations;
using InnovationLab.Landing.Models;
using Mapster;

namespace InnovationLab.Landing.Dtos.EventAgendas;

[AdaptTo(typeof(EventAgenda))]
public record EventAgendaCreateDto
(
    [Required][Range(1, int.MaxValue)] int Day,
    [Required][MinLength(1)] IList<AgendaItemCreateDto> Items
);