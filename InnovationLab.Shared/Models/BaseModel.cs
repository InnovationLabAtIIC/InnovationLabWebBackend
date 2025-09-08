namespace InnovationLab.Shared.Models;

public abstract class BaseModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; } = null;
    public DateTimeOffset? DeletedAt { get; set; } = null;
}