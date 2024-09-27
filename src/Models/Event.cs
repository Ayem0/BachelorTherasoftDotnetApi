using BachelorTherasoftDotnetApi.src.Base;

namespace BachelorTherasoftDotnetApi.src.Models;

public class Event : BaseModel
{
    public Event(string? description, DateTime startDate, DateTime endDate, Room room, EventCategory eventCategory, List<Participant> participants, List<Tag> tags)
    {
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        Room = room;
        RoomId = room.Id;
        EventCategory = eventCategory;
        EventCategoryId = eventCategory.Id;
        Participants = participants;
        Tags = tags;
        // TODO ajouter la liste des Users
    }
    public Event(string? description, DateTime startDate, DateTime endDate, string roomId, string eventCategoryId)
    {
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        RoomId = roomId;
        EventCategoryId = eventCategoryId;
    }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Description { get; set; }
    public string RoomId { get; set; }
    public required Room Room { get; set; }
    public string EventCategoryId { get; set; }
    public required EventCategory EventCategory { get; set; }
    public List<Participant> Participants { get; set; } = [];
    public List<User> Users { get; set; } = [];
    public List<Tag> Tags { get; set; } = [];
}

