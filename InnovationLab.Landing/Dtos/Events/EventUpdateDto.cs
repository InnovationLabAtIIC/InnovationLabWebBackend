using System.ComponentModel.DataAnnotations;

namespace InnovationLab.Landing.Dtos.Events;

public record EventUpdateDto(
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