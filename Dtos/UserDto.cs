using System;
using System.Text.Json.Serialization;

namespace BachelorTherasoftDotnetApi.Dtos;

public class UserDto
{
    public string? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<WorkspaceDto>? Workspaces { get; set; }
}
