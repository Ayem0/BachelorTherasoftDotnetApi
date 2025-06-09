namespace BachelorTherasoftDotnetApi.src.Dtos.Update;

public class UpdateEventRequest
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? RoomId { get; set; }
    public string? Description { get; set; }
    public string? EventCategoryId { get; set; }
    public List<string>? ParticipantIds { get; set; }
    public List<string>? TagIds { get; set; }
    public List<string>? UserIds { get; set; }
}
