using AutoMapper;
using InnovationLab.Landing.Models;
using System.ComponentModel.DataAnnotations;

namespace InnovationLabBackend.Api.Dtos.Faqs
{
    [AutoMap(typeof(Faq), ReverseMap = true)]
    public class CreateFaqDto
    {
        [Required]
        public string Question { get; set; } = default!;

        [Required]
        public string Answer { get; set; } = default!;

        public Guid CategoryId { get; set; }
    }
}
