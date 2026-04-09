using System.Text.Json.Serialization;

namespace InnovationLab.Landing.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EventRegistrationStatus
{
    Pending,
    Approved,
    Rejected
}