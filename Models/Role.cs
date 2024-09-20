using Microsoft.AspNetCore.Identity;

namespace BachelorTherasoftDotnetApi.Models;

public class Role : IdentityRole
{
    public DateTime CreatedAt = DateTime.Now;
    public DateTime ?DeletedAt { get; set; }
    public DateTime ?ModifiedAt { get; set; }
}

