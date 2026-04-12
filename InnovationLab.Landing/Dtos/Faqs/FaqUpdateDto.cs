using System.ComponentModel.DataAnnotations;
using InnovationLab.Landing.Models;
using Mapster;

namespace InnovationLab.Landing.Dtos.Faqs;

[AdaptTo(typeof(Faq))]
public record FaqUpdateDto
(
    [Required] string Question,
    [Required] string Answer,
    [Required] Guid CategoryId
);
