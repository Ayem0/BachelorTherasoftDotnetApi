using BachelorTherasoftDotnetApi.Classes;

namespace BachelorTherasoftDotnetApi.Models;

public class Event : DefaultFields
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required TimeOnly StartTime { get; set; }
    public required TimeOnly EndTime { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public string ?Description { get; set; }
    public required string RoomId { get; set; }
    public required Room Room { get; set; }
    public required string EventCategoryId { get; set; }
    public required EventCategory EventCategory { get; set; }
    public List<Participant> Participants { get; set; } = [];
    public List<User> Users { get; set; } = [];
    public List<Tag> Tags { get; set; } = [];
}

