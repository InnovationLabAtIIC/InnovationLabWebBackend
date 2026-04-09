using System.ComponentModel.DataAnnotations;
using AutoMapper;
using InnovationLabBackend.Api.Models;

namespace InnovationLabBackend.Api.Dtos.EventAgendas
{
    [AutoMap(typeof(AgendaItem), ReverseMap = true)]
    public class UpdateAgendaItemDto
    {
        [Required] public required TimeOnly StartTime { get; set; }
        [Required] public required TimeOnly EndTime { get; set; }
        [Required] public required string Title { get; set; }
        [Required] public required string Description { get; set; }
    }
}