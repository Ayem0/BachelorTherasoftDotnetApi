namespace BachelorTherasoftDotnetApi.src.Dtos;

public class CreateTagRequest
{
    public required string WorkspaceId { get; set; }
    public required string Name { get; set; }
    public required string Icon { get; set; }

}
