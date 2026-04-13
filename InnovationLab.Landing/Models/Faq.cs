using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InnovationLab.Shared.Models;

namespace InnovationLab.Landing.Models;

public class Faq : BaseModel
{
    [Required] public required string Question { get; set; }
    [Required] public required string Answer { get; set; }
    [Required] public required Guid CategoryId { get; set; }
    [ForeignKey(nameof(CategoryId))] public required Category Category { get; set; }
}
