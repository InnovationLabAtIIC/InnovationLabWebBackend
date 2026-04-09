using InnovationLab.Landing.Enums;
using InnovationLab.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace InnovationLab.Landing.Models;

public class Banner : BaseModel
{
    [Required][Url] public required string Url { get; set; }
    [Required] public MediaType Type { get; set; }
    [Required] public required string Title { get; set; }
    [Required] public required string SubTitle { get; set; }
    [Required] public required string Caption { get; set; }
    public bool Current { get; set; }
    public int Version { get; set; }
    public string? ParentId { get; set; }
    public DateTimeOffset? ScheduledStart { get; set; }
    public DateTimeOffset? ScheduledEnd { get; set; }

}
