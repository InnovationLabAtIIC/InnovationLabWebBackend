using AutoMapper;
using InnovationLabBackend.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace InnovationLabBackend.Api.Dtos.Categories
{
    [AutoMap(typeof(Category), ReverseMap = true)]
    public class CreateCategoryDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public Guid? ParentCategoryId { get; set; }
    }
}
