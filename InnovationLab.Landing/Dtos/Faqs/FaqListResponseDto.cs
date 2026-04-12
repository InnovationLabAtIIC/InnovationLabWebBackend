namespace InnovationLab.Landing.Dtos.Faqs;

public record FaqListResponseDto
(
    int Page,
    int Limit,
    int TotalItems,
    List<FaqResponseDto> Data
);
