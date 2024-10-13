using BachelorTherasoftDotnetApi.src.Enums;

namespace BachelorTherasoftDotnetApi.src.Dtos.Models;

public class InvitationDto
{
    public string Id { get; set; } = string.Empty;
    public InvitationType InvitationType { get; set; } 
    public EventDto? Event { get; set; }
    public WorkspaceDto? Workspace { get; set; }
    public UserDto Sender { get; set; } = new();
    public UserDto Receiver { get; set; } = new();
}
