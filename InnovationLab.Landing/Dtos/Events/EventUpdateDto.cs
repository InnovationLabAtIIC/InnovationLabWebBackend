using System.ComponentModel.DataAnnotations;
using InnovationLab.Landing.Models;
using Mapster;

namespace InnovationLab.Landing.Dtos.Events;

[AdaptTo(typeof(Event))]
public record EventUpdateDto
(
    Guid? ParentEventId,
    [Required] string Title,
    [Required] string Description,
    [Required][MinLength(1)][MaxLength(6)] IList<string> Highlights,
    [Required] DateTimeOffset StartTime,
    [Required] DateTimeOffset EndTime,
    [Required] string Location,
    IFormFile? CoverImage,
    string? SeriesName,
    [Required] bool IsTeamEvent,
    int? MaxTeamMembers,
    DateTimeOffset? RegistrationStart,
    DateTimeOffset? RegistrationEnd
);