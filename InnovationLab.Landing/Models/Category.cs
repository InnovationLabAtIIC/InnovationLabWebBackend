using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InnovationLab.Shared.Models;

namespace InnovationLab.Landing.Models;

public class Category : BaseModel
{
    [Required] public required string Name { get; set; }
    public Guid? ParentCategoryId { get; set; }
    [ForeignKey(nameof(ParentCategoryId))] public Category? ParentCategory { get; set; }
    public IEnumerable<Category> Subcategories { get; set; } = [];
    public IEnumerable<Faq> Faqs { get; set; } = [];
}
