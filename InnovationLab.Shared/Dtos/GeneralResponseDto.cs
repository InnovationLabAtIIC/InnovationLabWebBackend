using System.Text.Json.Serialization;

namespace InnovationLab.Shared.Dtos;

public record GeneralResponseDto<T>
{
    public required int StatusCode { get; init; }
    public required string Message { get; init; }
    public T? Result { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Page { get; init; } = null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Items { get; init; } = null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? TotalPages { get; init; } = null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? TotalItems { get; init; } = null;
}

public record GeneralResponseDto : GeneralResponseDto<object?>;