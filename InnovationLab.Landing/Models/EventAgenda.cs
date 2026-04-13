using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InnovationLab.Landing.Validations;
using InnovationLab.Shared.Models;

namespace InnovationLab.Landing.Models;

public class EventAgenda : BaseModel
{
    [Required] public required Guid EventId { get; set; }
    [ForeignKey(nameof(EventId))] public Event? Event { get; set; }
    [Required][ValidAgendaDay] public required int Day { get; set; }
    [Required][MinLength(1)] public required IEnumerable<AgendaItem> Items { get; set; }
}