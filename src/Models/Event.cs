﻿using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Enums;

namespace BachelorTherasoftDotnetApi.src.Models;

public class Event : BaseModel
{
    public Event(string? description, DateTime startDate, DateTime endDate, Room room, EventCategory eventCategory, List<Participant> participants, 
        List<Tag> tags, Interval? repetitionInterval, int? repetitionNumber, Event? mainEvent, DateOnly? repetitionEndDate)
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
        RepetitionInterval = repetitionInterval;
        RepetitionNumber = repetitionNumber;
        MainEvent = mainEvent;
        MainEventId = mainEvent?.Id;
        RepetitionEndDate = repetitionEndDate;
        // TODO ajouter la liste des Users
    }
    public Event(string? description, DateTime startDate, DateTime endDate, string roomId, string eventCategoryId, Interval? repetitionInterval, 
    int? repetitionNumber, string? mainEventId, DateOnly? repetitionEndDate)
    {
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




    public List<Participant> Participants { get; set; } = [];
    public List<EventUser> Users { get; set; } = [];
    public List<Tag> Tags { get; set; } = [];
}

