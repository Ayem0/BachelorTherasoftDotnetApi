using Microsoft.AspNetCore.Identity;

namespace BachelorTherasoftDotnetApi.src.Models;

public class Role : IdentityRole
{
    public DateTime CreatedAt = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}

