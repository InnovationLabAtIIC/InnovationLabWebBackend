using System.ComponentModel.DataAnnotations;
using InnovationLab.Landing.Enums;
using InnovationLab.Landing.Models;
using Mapster;

namespace InnovationLab.Landing.Dtos.EventRegistrations;

[AdaptTo(typeof(EventRegistration))]
public record EventRegistrationUpdateDto
(
    [Required] EventRegistrationStatus Status
);