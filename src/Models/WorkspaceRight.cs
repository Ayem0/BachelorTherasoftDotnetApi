using BachelorTherasoftDotnetApi.src.Base;

namespace BachelorTherasoftDotnetApi.src.Models;

public class WorkspaceRight : BaseModel
{
    public required string WorkspaceId { get; set; }
    public required Workspace Workspace { get; set; }
    public required string Name { get; set; }
    public virtual List<WorkspaceRole> WorkspaceRoles { get; set; } = [];
}
