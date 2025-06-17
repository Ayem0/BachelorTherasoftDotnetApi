using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BachelorTherasoftDotnetApi.src.Hubs;

[Authorize]
public class GlobalHub : Hub
{
    private readonly IWorkspaceService _workspaceService;
    private readonly ILogger<GlobalHub> _logger;
    public GlobalHub(IWorkspaceService workspaceService, ILogger<GlobalHub> logger)
    {
        _workspaceService = workspaceService;
        _logger = logger;
    }

    public override async Task<Task> OnConnectedAsync()
    {
        _logger.LogDebug("OnConnectedAsync : connectionId '{connectionId}', userId '{userId}'", Context.ConnectionId, Context.UserIdentifier);
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            var workspaces = await _workspaceService.GetByUserIdAsync(userId);
            foreach (var workspace in workspaces)
            {
                await AddToGroupAsync(workspace.Id);
            }
        }
        return base.OnConnectedAsync();
    }

    public override async Task<Task> OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogDebug("OnDisconnectedAsync : connectionId '{connectionId}', userId '{userId}'", Context.ConnectionId, Context.UserIdentifier);
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            var workspaces = await _workspaceService.GetByUserIdAsync(userId);
            foreach (var workspace in workspaces)
            {
                await RemoveFromGroupAsync(workspace.Id);
            }
        }
        return base.OnDisconnectedAsync(exception);
    }

    public Task AddToGroupAsync(string groupName)
    {
        _logger.LogDebug("AddToGroupAsync : connectionId '{connectionId}', groupName '{groupName}'", Context.ConnectionId, groupName);
        return Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public Task RemoveFromGroupAsync(string groupName)
    {
        _logger.LogDebug("AddToGroupAsync : connectionId '{connectionId}', groupName '{groupName}'", Context.ConnectionId, groupName);
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
}
