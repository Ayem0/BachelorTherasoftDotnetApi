using BachelorTherasoftDotnetApi.src.Enums;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Models;
[Keyless]
public class EventUser
{
    public EventUser(User user, Event @event)
    {
        User = user;
        UserId = user.Id;
        Event = @event;
        EventId = @event.Id;
        Status = Status.Pending;
    }
    public EventUser(string userId, string eventId, Status status) // maybe no need du status ici
    {
        UserId = userId;
        EventId = eventId;
        Status = status;
    }
    public required User User { get; set; }
    public string UserId { get; set; }
    public required Event Event { get; set; }
    public string EventId { get; set; }
    public Status Status { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
