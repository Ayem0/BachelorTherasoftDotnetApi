using BachelorTherasoftDotnetApi.Base;

namespace BachelorTherasoftDotnetApi.Models;

public class ParticipantCategory : BaseModel
{
    public required string WorkspaceId { get; set; }
    public required Workspace Workspace { get; set; }
    public required string Name { get; set; }
    public required string Icon { get; set; }

    public List<Participant> Participants { get; set; } = [];
}
