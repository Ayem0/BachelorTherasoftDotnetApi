using System;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BachelorTherasoftDotnetApi.src.Hubs;

[Authorize]
public class GlobalHub : Hub
{
    private static readonly ConnectionMapping _connections = new();

    public override Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            _connections.Add(userId, Context.ConnectionId);
        }
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            _connections.Remove(userId, Context.ConnectionId);
        }
        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendWorkspaceInvitation(InvitationDto invitation)
    {
        var receiverId = invitation.Receiver.Id;

        var connections = _connections.GetConnections(receiverId);

        foreach (var connectionId in connections)
        {
            await Clients.Client(connectionId).SendAsync("AddInvitation", invitation);
        }
    }

    public async Task RemoveInvitation(InvitationDto invitation) {
        var senderId = invitation.Sender.Id;

        var connections = _connections.GetConnections(senderId);

        foreach (var connectionId in connections)
        {
            await Clients.Client(connectionId).SendAsync("RemoveInvitation", invitation);
        }
    }
}
