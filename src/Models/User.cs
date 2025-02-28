using Microsoft.AspNetCore.Identity;

namespace BachelorTherasoftDotnetApi.src.Models;

public class User : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DisabledAt { get; set; }
    public List<Workspace> Workspaces { get; set; } = [];
    public List<WorkspaceRole> WorkspaceRoles { get; set; } = [];
    public List<EventUser> Events { get; set; } = [];
    public List<User> Contacts { get; set; } = [];
    public List<User> BlockedUsers { get; set; } = [];
}

