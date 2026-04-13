using System.ComponentModel.DataAnnotations;
using InnovationLab.Landing.Models;
using Mapster;
using InnovationLab.Landing.Enums;

namespace InnovationLab.Landing.Dtos.EventRegistrations;

[AdaptTo(typeof(EventRegistration))]
public record EventRegistrationUpdateDto
(
    [Required] EventRegistrationStatus Status
);