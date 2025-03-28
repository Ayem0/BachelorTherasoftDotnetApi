namespace BachelorTherasoftDotnetApi.src.Base;

public class BaseEntity : IBaseEntity
{
    public string Id { get; } = Guid.NewGuid().ToString();
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
