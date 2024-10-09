using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Enums;

namespace BachelorTherasoftDotnetApi.src.Models;

public class Invitation : BaseModel
{
    public Invitation(string id, InvitationType invitationType, string? workspaceId, string? eventId, string senderUserId, string receiverUserId)
    {
        Id = id;
        InvitationType = invitationType;
        WorkspaceId = workspaceId;
        EventId = eventId;
        SenderUserId = senderUserId;
        ReceiverUserId = receiverUserId;
    }
    public InvitationType InvitationType { get; set; } 
    public string? WorkspaceId { get; set; }
    public string? EventId { get; set; }
    public string SenderUserId { get; set; }
    public string ReceiverUserId { get; set; }

}
