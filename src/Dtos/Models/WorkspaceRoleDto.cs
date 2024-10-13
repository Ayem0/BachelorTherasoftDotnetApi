using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos.Models;

public class WorkspaceRoleDto
{
    [Required]
    public string Id { get; set; } = string.Empty;
    [Required]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // public List<UserDto>? Users { get; set; }
}
