using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Enums;

namespace BachelorTherasoftDotnetApi.src.Models;

public class Member : BaseModel
{
    public Member(User user, Workspace workspace)
    {
        User = user;
        Workspace = workspace;
        UserId = user.Id;
        WorkspaceId = workspace.Id;
        Status = Status.Pending;
    }
    public Member(string userId, string workspaceId, Status status)
    {
        UserId = userId;
        WorkspaceId = workspaceId;
        Status = status;
    }
    public string UserId { get; set; }
    public required User User { get; set; }
    public string WorkspaceId { get; set; }
    public required Workspace Workspace { get; set; }
    public Status Status { get; set; }

    public List<WorkspaceRole> Roles { get; set; } = [];
    public List<EventMember> Events { get; set; } = [];
    // TODO ajouter le json des horaires de travail
}
