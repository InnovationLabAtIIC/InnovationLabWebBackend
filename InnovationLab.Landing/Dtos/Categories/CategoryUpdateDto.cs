using System.ComponentModel.DataAnnotations;
using InnovationLab.Landing.Models;
using Mapster;

namespace InnovationLab.Landing.Dtos.Categories;

[AdaptTo(typeof(Category))]
public record CategoryUpdateDto
(
    [Required] string Name,
    Guid? ParentCategoryId
);
