using BachelorTherasoftDotnetApi.src.Base;

namespace BachelorTherasoftDotnetApi.src.Models;

public class Room : BaseModel
{
    public Room(Area area, string name, string? description)
    {
        // TODO ajouter la list<slot>
        Area = area;
        AreaId = area.Id;
        Name = name;
        Description = description;
    }

    public Room(string areaId, string name, string? description)
    {
        AreaId = areaId;
        Name = name;
        Description = description;
    }

    public string AreaId { get; set; }
    public required Area Area { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<Event> Events { get; set; } = [];
    public List<Slot> Slots { get; set; } = [];
}
