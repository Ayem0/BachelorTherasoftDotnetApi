using BachelorTherasoftDotnetApi.src.Base;
using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.src.Models;

public class ParticipantCategory : BaseEntity, IBaseEntity
{
    [Key]
    public string Id { get; set; } = string.Empty;
    public ParticipantCategory() { }
    public ParticipantCategory(Workspace workspace, string name, string? description, string color)
    {
        Workspace = workspace;
        WorkspaceId = workspace.Id;
        Name = name;
        Description = description;
        Color = color;
    }
    public ParticipantCategory(string workspaceId, string name, string? description, string color)
    {
        WorkspaceId = workspaceId;
        Name = name;
        Description = description;
        Color = color;
    }

    public string WorkspaceId { get; set; }
    public required Workspace Workspace { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Color { get; set; }

    public List<Participant> Participants { get; set; } = [];
}
