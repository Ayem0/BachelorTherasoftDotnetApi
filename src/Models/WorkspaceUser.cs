using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Models;
[Keyless]
public class WorkspaceUser
{
    public WorkspaceUser(User user, Workspace workspace)
    {
        User = user;
        Workspace = workspace;
        UserId = user.Id;
        WorkspaceId = workspace.Id;
    }
    public WorkspaceUser(string userId, string workspaceId)
    {
        UserId = userId;
        WorkspaceId = workspaceId;

    }
    public string UserId { get; set; }
    public required User User { get; set; }
    public string WorkspaceId { get; set; }
    public required Workspace Workspace { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
