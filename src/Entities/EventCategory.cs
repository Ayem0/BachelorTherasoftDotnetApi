using BachelorTherasoftDotnetApi.src.Base;
using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.src.Models;

public class EventCategory : BaseEntity, IBaseEntity
{
    [Key]
    public string Id { get; set; } = string.Empty;
    public EventCategory() { }
    public EventCategory(Workspace workspace, string name, string color, string? description)
    {
        Workspace = workspace;
        WorkspaceId = workspace.Id;
        Name = name;
        Color = color;
        Description = description;
    }
    public EventCategory(string workspaceId, string name, string color, string? description)
    {
        WorkspaceId = workspaceId;
        Name = name;
        Color = color;
        Description = description;
    }

    public string WorkspaceId { get; set; }
    public required Workspace Workspace { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public string? Description { get; set; }
    public List<Event> Events { get; set; } = [];
    public List<Slot> Slots { get; set; } = [];

}


