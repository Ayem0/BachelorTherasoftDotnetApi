namespace BachelorTherasoftDotnetApi.src.Dtos.Create;

public class CreateTagRequest
{
    public required string WorkspaceId { get; set; }
    public required string Name { get; set; }
    public required string Icon { get; set; }
    public required string Color { get; set; }
    public string? Description { get; set; }

}
