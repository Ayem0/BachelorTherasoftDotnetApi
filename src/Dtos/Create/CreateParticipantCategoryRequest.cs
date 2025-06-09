namespace BachelorTherasoftDotnetApi.src.Dtos.Create;

public class CreateParticipantCategoryRequest
{
    public required string WorkspaceId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Color { get; set; }
}
