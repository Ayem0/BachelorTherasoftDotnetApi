using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BachelorTherasoftDotnetApi.Dtos;

public class WorkspaceRoleDto
{
    [Required]
    public required string Id { get; set; }
    [Required]
    public required string Name { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<UserDto>? Users { get; set; }
}
