using BachelorTherasoftDotnetApi.src.Base;
using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.src.Models;

public class Area : BaseEntity, IBaseEntity
{
    [Key]
    public string Id { get; set; } = string.Empty;
    public Area() { }
    public Area(Workspace workspace, Location location, string name, string? description)
    {
        Location = location;
        LocationId = location.Id;
        Workspace = workspace;
        WorkspaceId = workspace.Id;
        Name = name;
        Description = description;
    }
    public Area(string workspaceId, string locationId, string name, string? description)
    {
        WorkspaceId = workspaceId;
        LocationId = locationId;
        Name = name;
        Description = description;
    }
    public string WorkspaceId { get; set; }
    public required Workspace Workspace { get; set; }
    public string LocationId { get; set; }
    public required Location Location { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<Room> Rooms { get; set; } = [];
}

