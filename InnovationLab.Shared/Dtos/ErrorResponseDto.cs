namespace InnovationLab.Shared.Dtos;

public record ErrorResponseDto(
    string TraceId,
    IEnumerable<string> Errors
);