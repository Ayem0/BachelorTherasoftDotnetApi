using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.src.Dtos.Models;

public class TagDto
{
    [Required]
    public string Id { get; set; } = string.Empty;
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Color { get; set; } = string.Empty;
    public string? Description { get; set; }
}

// public class TagJoinWorkspaceDto : TagDto
// {
//     public TagJoinWorkspaceDto(Tag tag) : base(tag)
//     {
//         Workspace = new WorkspaceDto(tag.Workspace);
//     }
//     public WorkspaceDto Workspace { get; set; }
// }