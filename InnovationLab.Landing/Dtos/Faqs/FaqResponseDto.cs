using InnovationLab.Landing.Models;
using Mapster;

namespace InnovationLab.Landing.Dtos.Faqs;

[AdaptFrom(typeof(Faq))]
public record FaqResponseDto
(
    Guid Id,
    string Question,
    string Answer,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt,
    Guid CategoryId,
    string CategoryName
);
