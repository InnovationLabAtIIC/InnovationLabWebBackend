using System.ComponentModel.DataAnnotations;
using InnovationLabBackend.Api.Enums;

namespace InnovationLabBackend.Api.Dtos.EventRegistrations
{
    public class UpdateEventRegistrationDto
    {
        [Required]
        public required EventRegistrationStatus Status { get; set; }
    }
}