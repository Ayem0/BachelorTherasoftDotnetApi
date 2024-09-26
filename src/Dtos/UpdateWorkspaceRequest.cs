using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class UpdateWorkspaceRequest
{
    [Required]
    public string? NewName { get; set; }
    public string? Description { get; set; }
}
