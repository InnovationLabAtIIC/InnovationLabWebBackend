using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InnovationLabBackend.Api.Models
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public required string Name { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }

        // Parent category reference (nullable for top-level categories)
        public Guid? ParentCategoryId { get; set; }

        [ForeignKey("ParentCategoryId")]
        public Category? ParentCategory { get; set; }

        // Collection of subcategories (children)
        public ICollection<Category> Subcategories { get; set; } = new List<Category>();

        public ICollection<Faq> Faqs { get; set; } = new List<Faq>();
    }
}
