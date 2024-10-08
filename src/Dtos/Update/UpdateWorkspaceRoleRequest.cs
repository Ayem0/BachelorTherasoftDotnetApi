using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.src.Dtos.Update;
public class UpdateWorkspaceRoleRequest
{
    [Required]
    public string? NewName { get; set; }
    public string? Description { get; set; }
}
