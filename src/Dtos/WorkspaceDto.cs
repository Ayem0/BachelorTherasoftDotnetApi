using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class WorkspaceDto
{
    [Required]
    public required string Id { get; set; }
    [Required]
    public required string Name { get; set; }

    public string? Description { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<UserDto>? Users { get; set; }
}
