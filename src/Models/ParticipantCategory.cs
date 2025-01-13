using BachelorTherasoftDotnetApi.src.Base;

namespace BachelorTherasoftDotnetApi.src.Models;

public class ParticipantCategory : BaseModel
{
    public ParticipantCategory(Workspace workspace, string name, string? description, string color, string icon)
    {
        Workspace = workspace;
        WorkspaceId = workspace.Id;
        Name = name;
        Icon = icon;
        Description = description;
        Color = color;
    }
    public ParticipantCategory(string workspaceId, string name, string? description, string color, string icon)
    {
        WorkspaceId = workspaceId;
        Name = name;
        Icon = icon;
        Description = description;
        Color = color;
    }

    public string WorkspaceId { get; set; }
    public required Workspace Workspace { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Color { get; set; }
    public string Icon { get; set; }

    public List<Participant> Participants { get; set; } = [];
}
