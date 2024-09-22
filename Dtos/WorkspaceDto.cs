using System;
using System.Text.Json.Serialization;
using BachelorTherasoftDotnetApi.Models;

namespace BachelorTherasoftDotnetApi.Dtos;

public class WorkspaceDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<UserDto>? Users { get; set; }
}
