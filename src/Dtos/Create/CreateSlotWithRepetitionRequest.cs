namespace BachelorTherasoftDotnetApi.src.Dtos.Create;

public class CreateSlotWithRepetitionRequest : RepetitionRequest
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string WorkspaceId { get; set; }
    public required DateOnly StartDate { get; set; }
    public required DateOnly EndDate { get; set; }
    public required TimeOnly StartTime { get; set; }
    public required TimeOnly EndTime { get; set; }
    public List<string>? EventCategoryIds { get; set; }
}
