namespace BachelorTherasoftDotnetApi.src.Dtos.Create;

public class CreateSlotWithRepetitionRequest : RepetitionRequest
{
    public required string WorkspaceId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public List<string>? EventCategoryIds { get; set; }
}
