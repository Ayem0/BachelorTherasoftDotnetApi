using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Enums;
using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.src.Models;

public class Invitation : BaseEntity, IBaseEntity
{
    [Key]
    public string Id { get; set; } = string.Empty;
    public Invitation() { }
    public Invitation(Event @event, User sender, User receiver)
    {
        InvitationType = InvitationType.Event;
        Workspace = null;
        Event = @event;
        WorkspaceId = null;
        EventId = @event.Id;
        SenderId = sender.Id;
        ReceiverId = receiver.Id;
        Sender = sender;
        Receiver = receiver;
    }
    public Invitation(Workspace workspace, User sender, User receiver)
    {
        InvitationType = InvitationType.Workspace;
        Workspace = workspace;
        Event = null;
        WorkspaceId = workspace.Id;
        EventId = null;
        SenderId = sender.Id;
        ReceiverId = receiver.Id;
        Sender = sender;
        Receiver = receiver;
    }

    public Invitation(User sender, User receiver)
    {
        InvitationType = InvitationType.Contact;
        Workspace = null;
        Event = null;
        WorkspaceId = null;
        EventId = null;
        SenderId = sender.Id;
        ReceiverId = receiver.Id;
        Sender = sender;
        Receiver = receiver;
    }

    public Invitation(InvitationType invitationType, string? workspaceId, string? eventId, string senderId, string receiverId)
    {
        InvitationType = invitationType;
        EventId = eventId;
        WorkspaceId = workspaceId;
        SenderId = senderId;
        ReceiverId = receiverId;
    }
    public InvitationType InvitationType { get; set; }
    public Workspace? Workspace { get; set; }
    public string? WorkspaceId { get; set; }
    public Event? Event { get; set; }
    public string? EventId { get; set; }
    public required User Sender { get; set; }
    public required User Receiver { get; set; }
    public string SenderId { get; set; }
    public string ReceiverId { get; set; }
}
