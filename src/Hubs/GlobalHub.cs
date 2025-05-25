using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BachelorTherasoftDotnetApi.src.Hubs;

[Authorize]
public class GlobalHub : Hub
{
    private readonly IWorkspaceService _workspaceService;
    public GlobalHub(IWorkspaceService workspaceService)
    {
        _workspaceService = workspaceService;
    }

    public override async Task<Task> OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            var workspaces = await _workspaceService.GetByUserIdAsync(userId);
            foreach (var workspace in workspaces)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, workspace.Id);
            }
        }

        return base.OnConnectedAsync();
    }

    public override async Task<Task> OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            var workspaces = await _workspaceService.GetByUserIdAsync(userId);
            foreach (var workspace in workspaces)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, workspace.Id);
            }
        }

        return base.OnDisconnectedAsync(exception);
    }

    public async Task NotifyGroup(string groupId, string key, object value)
    {
        await Clients.Group(groupId).SendAsync(key, value);
    }

    public async Task NotifyUser(string userId, string key, object value)
    {
        await Clients.User(userId).SendAsync(key, value);
    }
}
