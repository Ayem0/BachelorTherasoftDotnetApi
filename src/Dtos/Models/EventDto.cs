using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BachelorTherasoftDotnetApi.src.Enums;

namespace BachelorTherasoftDotnetApi.src.Dtos.Models;

public class EventDto
{
    [Required]
    public required string Id { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Description { get; set; }
    public RoomDto? Room { get; set; }
    public WorkspaceDto? Workspace { get; set; }
    public EventCategoryDto? EventCategory { get; set; }
    public Interval? RepetitionInterval { get; set; }
    public int? RepetitionNumber { get; set; }
    public EventDto? MainEvent { get; set; }
    public string? MainEventId { get; set; }
    public DateOnly? RepetitionEndDate { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<ParticipantDto>? Participants { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<UserDto>? Members { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<TagDto>? Tags { get; set; }
}
