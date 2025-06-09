namespace BachelorTherasoftDotnetApi.src.Dtos.Create;

public class CreateWorkspaceRequest
{
    public required string Name { get; set; }
    public required string Color { get; set; }
    public string? Description { get; set; }
}
