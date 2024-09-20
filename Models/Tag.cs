using BachelorTherasoftDotnetApi.Classes;

namespace BachelorTherasoftDotnetApi.Models;

public class Tag : DefaultFields
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string WorkspaceId { get; set; }
    public required Workspace Workspace { get; set; }

    public required string Name { get; set; }
    public required string Icon { get; set; }

    public List<Event> Events { get; set; } = [];
}
