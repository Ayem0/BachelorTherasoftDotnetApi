using System;

namespace BachelorTherasoftDotnetApi.src.Hubs;

public interface IHubService
{
    public Task NotififyUser(string userId, string key, object value);
    public Task NotififyUsers(IEnumerable<string> userIds, string key, object value);
    public Task NotififyGroup(string groupId, string key, object value);

}
