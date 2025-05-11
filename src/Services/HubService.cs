using System;
using BachelorTherasoftDotnetApi.src.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace BachelorTherasoftDotnetApi.src.Services;

public class HubService : IHubService
{
    private readonly IHubContext<GlobalHub> _hub;
    private readonly ILogger _logger;
    public HubService(IHubContext<GlobalHub> hub, ILogger<HubService> logger)
    {
        _hub = hub;
        _logger = logger;
    }

    public Task NotififyGroup(string groupId, string key, object value)
    {
        _logger.LogInformation("Notififying group '{groupId}' with key '{key}' with value '{value}'", groupId, key, value);
        return _hub.Clients.Group(groupId).SendAsync(key, value);
    }

    public Task NotififyUser(string userId, string key, object value)
    {
        _logger.LogInformation("Notififying user '{userId}' with key '{key}' with value '{value}'", userId, key, value);
        return _hub.Clients.User(userId).SendAsync(key, value);
    }

    public Task NotififyUsers(IEnumerable<string> userIds, string key, object value)
    {
        _logger.LogInformation("Notififying users '{userIds}' with key '{key}' with value '{value}'", userIds, key, value);
        return _hub.Clients.Users(userIds).SendAsync(key, value);
    }
}
