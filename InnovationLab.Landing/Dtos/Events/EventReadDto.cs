namespace InnovationLab.Landing.Dtos.Events;

public record EventReadDto(
    Guid Id,
    Guid? ParentEventId,
    string Title,
    string Description,
    IList<string> Highlights,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime,
    string Location,
    string CoverImageUrl,
    string? SeriesName,
    bool IsTeamEvent,
    int MaxTeamMembers,
    DateTimeOffset? RegistrationStart,
    DateTimeOffset? RegistrationEnd,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);