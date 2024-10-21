using System.ComponentModel.DataAnnotations;
using BachelorTherasoftDotnetApi.src.Enums;

namespace BachelorTherasoftDotnetApi.src.Dtos.Models;

public class EventDto
{
    [Required]
    public string Id { get; set; } = string.Empty;
    [Required]
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Description { get; set; }
    public required RoomDto Room { get; set; }
    public required EventCategoryDto EventCategory { get; set; }
    public Interval? RepetitionInterval { get; set; }
    public int? RepetitionNumber { get; set; }
    public EventDto? MainEvent { get; set; }
    public string? MainEventId { get; set; }
    public DateOnly? RepetitionEndDate { get; set; }




    public List<ParticipantDto> Participants { get; set; } = [];
    public List<UserDto> Users { get; set; } = [];
    public List<TagDto> Tags { get; set; } = [];
}
