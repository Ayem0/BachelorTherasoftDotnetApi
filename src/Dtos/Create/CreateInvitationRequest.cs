using System;
using BachelorTherasoftDotnetApi.src.Enums;

namespace BachelorTherasoftDotnetApi.src.Dtos.Create;

public class CreateInvitationRequest
{
    public required InvitationType InvitationType { get; set; } 
    public string? WorkspaceId { get; set; }
    public string? EventId { get; set; }
    public required string SenderUserId { get; set; }
    public required string ReceiverUserId { get; set; }
}
