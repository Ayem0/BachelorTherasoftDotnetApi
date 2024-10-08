namespace BachelorTherasoftDotnetApi.src.Dtos.Create;

public class CreateLocationRequest
{
    public required string WorkspaceId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
}
