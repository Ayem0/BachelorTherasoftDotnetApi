using BachelorTherasoftDotnetApi.Base;

namespace BachelorTherasoftDotnetApi.Models;

public class Room : BaseModel
{
    public required string AreaId { get; set; }
    public required Area Area { get; set; }
    public required string Name { get; set; }
    public List<Event> Events { get; set; } = [];
    public List<Slot> Slots { get; set; } = [];
}
