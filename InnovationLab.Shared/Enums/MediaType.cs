using System.Text.Json.Serialization;

namespace InnovationLab.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MediaType
{
    Image,
    Video,
    Pdf,
    NotSupported
}
