using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos.Models;

public class WorkspaceRoleDto
{
    public WorkspaceRoleDto(WorkspaceRole workspaceRole)
    {
        Id = workspaceRole.Id;
        Name = workspaceRole.Name;
        Description = workspaceRole.Description;
    }
    [Required]
    public string Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
    
    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // public List<UserDto>? Users { get; set; }
}
