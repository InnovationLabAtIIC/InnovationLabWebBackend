using InnovationLab.Landing.Enums;
using System.ComponentModel.DataAnnotations;

namespace InnovationLab.Landing.Dtos.EventRegistrations;

public record EventRegistrationFilterDto
(
    EventRegistrationStatus? Status,
    [Range(1, int.MaxValue)] int Page = 1,
    [Range(1, 100)] int Limit = 10
);