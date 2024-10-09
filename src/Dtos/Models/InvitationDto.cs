using System;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos.Models;

public class InvitationDto
{
    public InvitationDto(Invitation invitation)
    {
        Id = invitation.Id;
        WorkspaceId = invitation.WorkspaceId;
        EventId = invitation.EventId;
        InvitationType = invitation.InvitationType;
        SenderUserId = invitation.SenderUserId;
        ReceiverUserId = invitation.ReceiverUserId;
    }

    public string Id { get; set; }
    public InvitationType InvitationType { get; set; } 
    public string? WorkspaceId { get; set; }
    public string? EventId { get; set; }
    public string SenderUserId { get; set; }
    public string ReceiverUserId { get; set; }
}
