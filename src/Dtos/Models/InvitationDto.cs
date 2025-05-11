using BachelorTherasoftDotnetApi.src.Enums;

namespace BachelorTherasoftDotnetApi.src.Dtos.Models;

public class InvitationDto
{
    public string Id { get; set; } = string.Empty;
    public InvitationType InvitationType { get; set; }
    public string? EventId { get; set; }
    public EventDto? Event { get; set; }
    public string? WorkspaceId { get; set; }
    public WorkspaceDto? Workspace { get; set; }
    public string SenderId { get; set; } = string.Empty;
    public UserDto Sender { get; set; } = new();
    public string ReceiverId { get; set; } = string.Empty;
    public UserDto Receiver { get; set; } = new();
}
