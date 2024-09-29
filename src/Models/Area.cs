using BachelorTherasoftDotnetApi.src.Base;

namespace BachelorTherasoftDotnetApi.src.Models;

public class Area : BaseModel
{
    public Area(Location location, string name, string? description)
    {
        Location = location;
        LocationId = location.Id;
        Name = name;
        Description = description;
    }
    public Area(string locationId, string name, string? description)
    {
        LocationId = locationId;
        Name = name;
        Description = description;
    }
    public string LocationId { get; set; }
    public required Location Location { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<Room> Rooms { get; set; } = [];
}

