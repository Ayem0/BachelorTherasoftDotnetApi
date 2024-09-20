using BachelorTherasoftDotnetApi.Classes;

namespace BachelorTherasoftDotnetApi.Models;

public class Area : DefaultFields
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string LocationId { get; set; }
    public required Location Location { get; set; }
    public required string Name { get; set; }

    public List<Room> Rooms { get; set; } = [];
}

