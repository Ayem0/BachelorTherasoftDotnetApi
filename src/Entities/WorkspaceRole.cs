using System;
using BachelorTherasoftDotnetApi.src.Base;

namespace BachelorTherasoftDotnetApi.src.Models;

public class WorkspaceRole : BaseEntity, BaseAuthorizationModel
{
    public WorkspaceRole(Workspace workspace, string name, string? description)
    {
        Workspace = workspace;
        WorkspaceId = workspace.Id;
        Name = name;
        Description = description;
    }
    public WorkspaceRole(string workspaceId, string name, string? description)
    {
        WorkspaceId = workspaceId;
        Name = name;
        Description = description;
    }
    public string WorkspaceId { get; set; }
    public required Workspace Workspace { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<User> Users { get; set; } = [];
}
