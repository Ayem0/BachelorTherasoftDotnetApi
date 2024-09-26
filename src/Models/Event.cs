using BachelorTherasoftDotnetApi.src.Base;

namespace BachelorTherasoftDotnetApi.src.Models;

public class Event : BaseModel
{
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public string? Description { get; set; }
    public required string RoomId { get; set; }
    public required Room Room { get; set; }
    public string? EventCategoryId { get; set; }
    public EventCategory? EventCategory { get; set; }
    public List<Participant> Participants { get; set; } = [];
    public List<User> Users { get; set; } = [];
    public List<Tag> Tags { get; set; } = [];
}

