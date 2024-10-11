using System.ComponentModel.DataAnnotations;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos.Models;

public class TagDto
{
    public TagDto(Tag tag)
    {
        Id = tag.Id;
        Name = tag.Name;
        Icon = tag.Icon;
    }
    [Required]
    public string Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Icon { get; set; }
}

public class TagJoinWorkspaceDto : TagDto
{
    public TagJoinWorkspaceDto(Tag tag) : base(tag)
    {
        Workspace = new WorkspaceDto(tag.Workspace);
    }
    public WorkspaceDto Workspace { get; set; }
}