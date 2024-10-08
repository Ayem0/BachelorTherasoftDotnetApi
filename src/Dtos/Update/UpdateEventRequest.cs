namespace BachelorTherasoftDotnetApi.src.Dtos.Update;
public class UpdateEventRequest
{
    public DateTime? NewStartDate { get; set; }
    public DateTime? NewEndDate { get; set; }
    public string? NewRoomId { get; set; }
    public string? NewDescription { get; set; }
    public string? NewEventCategoryId { get; set; }

    public List<string>? NewParticipantIds { get; set; }
    public List<string>? NewTagIds { get; set; }
}
