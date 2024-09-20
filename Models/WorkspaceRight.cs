using BachelorTherasoftDotnetApi.Classes;

namespace BachelorTherasoftDotnetApi.Models;

public class WorkspaceRight : DefaultFields
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string WorkspaceId { get; set; }
    public required Workspace Workspace { get; set; }
    public required string Name { get; set; }
    public List<WorkspaceRole> WorkspaceRoles { get; set; } = [];
}
