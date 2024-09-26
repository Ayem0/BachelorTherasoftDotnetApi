using System.Text.Json.Serialization;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class UserDto
{
    public required string Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<WorkspaceDto>? Workspaces { get; set; }
}
