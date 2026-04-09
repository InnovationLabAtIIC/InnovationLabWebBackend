namespace InnovationLabBackend.Api.Dtos.Faqs
{
    public class PaginatedFaqResponseDto
    {
        public int Page { get; set; }
        public int Limit { get; set; }
        public int TotalItems { get; set; }
        public List<FaqResponseDto> Data { get; set; } = new();

    }
}
