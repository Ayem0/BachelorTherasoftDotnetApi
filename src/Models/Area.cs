using BachelorTherasoftDotnetApi.src.Base;

namespace BachelorTherasoftDotnetApi.src.Models;

public class Area : BaseModel
{
    public required string LocationId { get; set; }
    public required Location Location { get; set; }
    public required string Name { get; set; }

    public List<Room> Rooms { get; set; } = [];
}

