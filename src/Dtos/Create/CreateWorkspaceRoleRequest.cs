namespace BachelorTherasoftDotnetApi.src.Dtos.Create;

public class CreateWorkspaceRoleRequest
{
    public required string Name { get ;set; }
    public required string WorkspaceId { get ;set; }
    public string? Description { get; set; }
}
