namespace BachelorTherasoftDotnetApi.src.Dtos.Create;

public class CreateEventWithRepetitionRequest : RepetitionRequest
{
    public required DateTime StartDate { get; set; }
    public required string WorkspaceId { get; set; }
    public required DateTime EndDate { get; set; }
    public required string RoomId { get; set; }
    public string? Description { get; set; }
    public required string EventCategoryId { get; set; }
    public List<string> ParticipantIds = [];
    public List<string> TagIds = [];
    public List<string> UserIds = [];
}
