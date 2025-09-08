using System.ComponentModel.DataAnnotations;
using InnovationLab.Landing.Dtos.Events;
using InnovationLab.Shared.Models;
using Mapster;

namespace InnovationLab.Landing.Models;

[AdaptTo(typeof(EventReadDto))]
[AdaptFrom(typeof(EventCreateDto))]
[AdaptFrom(typeof(EventUpdateDto))]
public class Event : BaseModel
{
    public Guid? ParentEventId { get; set; }
    public Event? ParentEvent { get; set; }
    [Required] public required string Title { get; set; }
    [Required] public required string Description { get; set; }
    [Required][MinLength(1)][MaxLength(6)] public required IList<string> Highlights { get; set; }
    [Required] public required DateTimeOffset StartTime { get; set; }
    [Required] public required DateTimeOffset EndTime { get; set; }
    [Required] public required string Location { get; set; }
    [Required] public required string CoverImageUrl { get; set; }
    public string? SeriesName { get; set; }
    [Required] public required bool IsTeamEvent { get; set; }
    [Required] public required int MaxTeamMembers { get; set; }
    public DateTimeOffset? RegistrationStart { get; set; }
    public DateTimeOffset? RegistrationEnd { get; set; }
}