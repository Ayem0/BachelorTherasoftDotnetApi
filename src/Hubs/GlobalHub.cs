using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BachelorTherasoftDotnetApi.src.Hubs;

[Authorize]
public class GlobalHub : Hub
{
    private readonly IUserService _userService;
    public GlobalHub(IUserService userService)
    {
        _userService = userService;
    }

    public override async Task<Task> OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;

        if (userId != null)
        {
            var user = await _userService.GetUserJoinWorkspacesByIdAsync(userId);
            if (user != null)
            {
                foreach (var workspace in user.Workspaces)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, workspace.Id);
                }
            }
        }
        return base.OnConnectedAsync();
    }

    public override async Task<Task> OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            var user = await _userService.GetUserJoinWorkspacesByIdAsync(userId);
            if (user != null)
            {
                foreach (var workspace in user.Workspaces)
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, workspace.Id);
                }
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
