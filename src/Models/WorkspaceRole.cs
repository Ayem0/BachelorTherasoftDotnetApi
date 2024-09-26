using System;
using BachelorTherasoftDotnetApi.src.Base;

namespace BachelorTherasoftDotnetApi.src.Models;

public class WorkspaceRole : BaseModel
{
    public required string WorkspaceId { get; set; }
    public required Workspace Workspace { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public virtual List<User> Users { get; set; } = [];
    public virtual List<WorkspaceRight> WorkspaceRights { get; set; } = [];
}
