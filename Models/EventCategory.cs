using BachelorTherasoftDotnetApi.Base;

namespace BachelorTherasoftDotnetApi.Models;

public class EventCategory : BaseModel
{
    public required string WorkspaceId { get; set; }
    public required Workspace Workspace { get; set; }
    public required string Name { get; set; }
    public required string Icon { get; set; }
    public List<Event> Events { get; set; } = [];
    public List<Slot> Slots { get; set; } = [];

}


