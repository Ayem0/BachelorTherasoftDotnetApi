using BachelorTherasoftDotnetApi.src.Base;

namespace BachelorTherasoftDotnetApi.src.Models;

public class Tag : BaseEntity, BaseAuthorizationModel
{
    public Tag() { }
    public Tag(Workspace workspace, string name, string icon, string color, string? description)
    {
        Workspace = workspace;
        WorkspaceId = workspace.Id;
        Name = name;
        Icon = icon;
        Description = description;
        Color = color;
    }
    public Tag(string workspaceId, string name, string icon, string color, string? description)
    {
        WorkspaceId = workspaceId;
        Name = name;
        Icon = icon;
        Description = description;
        Color = color;
    }

    public string WorkspaceId { get; set; }
    public required Workspace Workspace { get; set; }
    public string Name { get; set; }
    public string Icon { get; set; }
    public string Color { get; set; }
    public string? Description { get; set; }
    public List<Event> Events { get; set; } = [];
}
