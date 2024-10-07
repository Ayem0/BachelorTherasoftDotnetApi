using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class CreateEventRequest
{
    [DataType(DataType.DateTime)]
    public required DateTime StartDate { get; set; }
    [DataType(DataType.DateTime)]
    public required DateTime EndDate { get; set; }
    public required string RoomId { get; set; }
    public string? Description { get; set; }
    public required string EventCategoryId { get; set; }
    public List<string>? ParticipantIds { get; set; }
    public List<string>? TagIds { get; set; }
}
