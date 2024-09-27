using BachelorTherasoftDotnetApi.src.Base;

namespace BachelorTherasoftDotnetApi.src.Models;

public class Tag : BaseModel
{
    public Tag(Workspace workspace, string name, string icon, string? description)
    {
        Workspace = workspace;
        WorkspaceId = workspace.Id;
        Name = name;
        Icon = icon;
        Description = description;
    }
    public Tag(string workspaceId, string name, string icon, string? description)
    {
        WorkspaceId = workspaceId;
        Name = name;
        Icon = icon;
        Description = description;
    }

    public string WorkspaceId { get; set; }
    public required Workspace Workspace { get; set; }
    public string Name { get; set; }
    public string Icon { get; set; }
    public string? Description { get; set; }
    public List<Event> Events { get; set; } = [];
}
