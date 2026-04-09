using System.ComponentModel.DataAnnotations;
using InnovationLab.Landing.Enums;

namespace InnovationLabBackend.Api.Dtos.EventRegistrations
{
    public class UpdateEventRegistrationDto
    {
        [Required]
        public required EventRegistrationStatus Status { get; set; }
    }
}