using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Enums;
using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.src.Models;

public class Event : BaseEntity, IBaseEntity
{
    [Key]
    public string Id { get; set; } = string.Empty;
    public Event() { }
    public Event(Workspace workspace, string? description, DateTime startDate, DateTime endDate, Room room, EventCategory eventCategory, List<Participant> participants, List<EventUser> users,
        List<Tag> tags, Interval? repetitionInterval, int? repetitionNumber, Event? mainEvent, DateOnly? repetitionEndDate)
    {
        Workspace = workspace;
        WorkspaceId = workspace.Id;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        Room = room;
        RoomId = room.Id;
        EventCategory = eventCategory;
        EventCategoryId = eventCategory.Id;
        Participants = participants;
        Tags = tags;
        RepetitionInterval = repetitionInterval;
        RepetitionNumber = repetitionNumber;
        MainEvent = mainEvent;
        MainEventId = mainEvent?.Id;
        RepetitionEndDate = repetitionEndDate;
        Users = users;
    }
    public Event(string workspaceId, string? description, DateTime startDate, DateTime endDate, string roomId, string eventCategoryId, Interval? repetitionInterval,
    int? repetitionNumber, string? mainEventId, DateOnly? repetitionEndDate)
    {
        WorkspaceId = workspaceId;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        RoomId = roomId;
        EventCategoryId = eventCategoryId;
        RepetitionInterval = repetitionInterval;
        RepetitionNumber = repetitionNumber;
        MainEventId = mainEventId;
        RepetitionEndDate = repetitionEndDate;
    }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Description { get; set; }
    public string RoomId { get; set; }
    public required Room Room { get; set; }
    public string EventCategoryId { get; set; }
    public required EventCategory EventCategory { get; set; }
    public Interval? RepetitionInterval { get; set; }
    public int? RepetitionNumber { get; set; }
    public Event? MainEvent { get; set; }
    public string? MainEventId { get; set; }
    public DateOnly? RepetitionEndDate { get; set; }
    public string WorkspaceId { get; set; }
    public required Workspace Workspace { get; set; }



    public List<Participant> Participants { get; set; } = [];
    public List<EventUser> Users { get; set; } = [];
    public List<Tag> Tags { get; set; } = [];
}

