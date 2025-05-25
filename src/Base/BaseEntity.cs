using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.src.Base;

public class BaseEntity
{
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
