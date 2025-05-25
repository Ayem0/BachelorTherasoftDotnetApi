namespace BachelorTherasoftDotnetApi.src.Base;

public class BaseEntity : IBaseEntity
{
    public string Id { get; set; } = string.Empty;
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
