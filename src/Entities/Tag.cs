using BachelorTherasoftDotnetApi.src.Base;
using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.src.Models;

public class Tag : BaseEntity, IBaseEntity
{
    [Key]
    public string Id { get; set; } = string.Empty;
    public Tag() { }
    public Tag(Workspace workspace, string name, string color, string? description)
    {
        Workspace = workspace;
        WorkspaceId = workspace.Id;
        Name = name;
        Description = description;
        Color = color;
    }
    public Tag(string workspaceId, string name, string color, string? description)
    {
        WorkspaceId = workspaceId;
        Name = name;
        Description = description;
        Color = color;
    }

    public string WorkspaceId { get; set; }
    public required Workspace Workspace { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public string? Description { get; set; }
    public List<Event> Events { get; set; } = [];
}
