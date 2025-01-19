using BachelorTherasoftDotnetApi.src.Base;

namespace BachelorTherasoftDotnetApi.src.Models;

public class Room : BaseModel, BaseAuthorizationModel
{
    public Room(Workspace workspace, Area area, string name, string? description)
    {
        Workspace = workspace;
        WorkspaceId = workspace.Id;
        // TODO ajouter la list<slot>
        Area = area;
        AreaId = area.Id;
        Name = name;
        Description = description;
    }

    public Room(string workspaceId, string areaId, string name, string? description)
    {
        WorkspaceId = workspaceId;
        AreaId = areaId;
        Name = name;
        Description = description;
    }
    public string WorkspaceId { get; set; }
    public required Workspace Workspace { get; set; }
    public string AreaId { get; set; }
    public required Area Area { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<Event> Events { get; set; } = [];
    public List<Slot> Slots { get; set; } = [];
}
