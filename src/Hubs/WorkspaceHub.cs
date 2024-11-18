using System;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BachelorTherasoftDotnetApi.src.Hubs;
[Authorize]
public class WorkspaceHub : Hub
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

    public async Task JoinWorkspaceGroup(string workspaceId) 
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, workspaceId);
    }

    public async Task LeaveWorkspaceGroup(string workspaceId) 
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, workspaceId);
        
    }
}