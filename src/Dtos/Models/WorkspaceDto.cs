using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos.Models;

public class WorkspaceDto
{
    public WorkspaceDto(Workspace workspace)
    {
        Id = workspace.Id;
        Name = workspace.Name;
        Description = workspace.Description;
    }
    [Required]
    public string Id { get; set; }
    [Required]
    public string Name { get; set; }

    public string? Description { get; set; }
    
    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // public List<UserDto>? Users { get; set; }
}
