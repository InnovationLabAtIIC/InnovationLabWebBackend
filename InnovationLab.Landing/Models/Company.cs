using InnovationLab.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace InnovationLab.Landing.Models;

[Index(nameof(Priority), IsUnique = true)]
public class Company : BaseModel
{
    public required string Name { get; set; }
    public required string Address { get; set; }
    public string? ContactEmail { get; set; }
    public required string LogoUrl { get; set; }
    public int Priority { get; set; }
}