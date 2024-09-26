using System;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class EventDto
{
    public required string Id { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public string? Description { get; set; }
    public required string RoomId { get; set; }
    public required RoomDto Room { get; set; }
    public string? EventCategoryId { get; set; }
    public EventCategory? EventCategory { get; set; } // TODO a changer par un dto
}
