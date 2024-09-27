namespace BachelorTherasoftDotnetApi.src.Dtos;

public class CreateEventRequest
{
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public required string RoomId { get; set; }
    public string? Description { get; set; }
    public required string EventCategoryId { get; set; }
    public List<string>? ParticipantIds { get; set; }
}
