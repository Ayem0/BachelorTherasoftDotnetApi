using BachelorTherasoftDotnetApi.src.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.src.Models;

public class WorkspaceRole : BaseEntity, IBaseEntity
{
    [Key]
    public string Id { get; set; } = string.Empty;
    public WorkspaceRole()
    {
    }
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
