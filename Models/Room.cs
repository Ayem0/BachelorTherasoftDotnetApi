using BachelorTherasoftDotnetApi.Classes;

namespace BachelorTherasoftDotnetApi.Models;

public class Room : DefaultFields
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string AreaId { get; set; }
    public required Area Area { get; set; }
    public required string Name { get; set; }
    public List<Event> Events { get; set; } = [];
    public List<Slot> Slots { get; set; } = [];
}
