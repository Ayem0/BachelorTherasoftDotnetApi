using System;
using BachelorTherasoftDotnetApi.Classes;

namespace BachelorTherasoftDotnetApi.Models;

public class WorkspaceRole : DefaultFields
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string WorkspaceId { get; set; }
    public required Workspace Workspace { get; set; }
    public required string Name { get; set; }
    public List<User> Users { get; set; } = [];
    public List<WorkspaceRight> WorkspaceRights { get; set; } = [];
}
