using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class CreateWorkspaceRoleRequest
{
    [Required]
    public required string Name { get ;set; }
    [Required]
    public required string WorkspaceId { get ;set; }
    public string? Description { get; set; }
}
