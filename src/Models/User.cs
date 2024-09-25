using Microsoft.AspNetCore.Identity;

namespace BachelorTherasoftDotnetApi.src.Models;

public class User : IdentityUser
{
    public string ?FirstName { get; set; }
    public string ?LastName { get; set; }
    public DateTime CreatedAt  { get; set; } = DateTime.Now;
    public DateTime ?DeletedAt { get; set; }
    public DateTime ?ModifiedAt { get; set; }
    public DateTime ?DisabledAt { get; set; }
    public virtual List<Workspace> Workspaces { get; set; } = [];
    public virtual List<WorkspaceRole> WorkspaceRoles { get; set; } = [];
    public virtual List<Event> Events { get; set; } = [];
}

