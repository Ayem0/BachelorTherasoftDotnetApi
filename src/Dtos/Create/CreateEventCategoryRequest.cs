namespace BachelorTherasoftDotnetApi.src.Dtos.Create;

public class CreateEventCategoryRequest
{
    public required string WorkspaceId { get; set; }
    public required string Name { get; set; }
    public required string Color { get; set; }
    public string? Description { get; set; }
}
