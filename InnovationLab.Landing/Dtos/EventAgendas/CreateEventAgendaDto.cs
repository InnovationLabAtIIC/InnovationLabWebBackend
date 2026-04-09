using System.ComponentModel.DataAnnotations;
using AutoMapper;
using InnovationLabBackend.Api.Models;

namespace InnovationLabBackend.Api.Dtos.EventAgendas
{
    [AutoMap(typeof(EventAgenda), ReverseMap = true)]
    public class CreateEventAgendaDto
    {
        [Required] public required int Day { get; set; }
        [Required] public required IList<CreateAgendaItemDto> Items { get; set; }
    }
}