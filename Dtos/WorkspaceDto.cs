using System;

namespace BachelorTherasoftDotnetApi.Dtos;

public class WorkspaceDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public List<UserDto> Users { get; set; } = [];
    public List<UserDto> WorkspaceRoles{ get; set; } = [];
}
