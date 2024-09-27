using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class UserDto
{
    public UserDto(User user)
    {
        Id = user.Id;
        FirstName = user.FirstName;
        LastName = user.LastName;
    }
    [Required]
    public string Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // public List<WorkspaceDto>? Workspaces { get; set; }
}
