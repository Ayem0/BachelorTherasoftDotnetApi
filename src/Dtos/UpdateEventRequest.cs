namespace BachelorTherasoftDotnetApi.src.Dtos;

public class UpdateEventRequest
{
    public DateTime? NewStartDate { get; set; }
    public DateTime? NewEndDate { get; set; }
    public string? NewRoomId { get; set; }
    public string? NewDescription { get; set; }
    public string? NewEventCategoryId { get; set; }

    public List<string>? NewParticipantIds { get; set; }
}
