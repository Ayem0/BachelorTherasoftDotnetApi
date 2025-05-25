using System;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface ISocketService
{
    public Task NotififyUser(string userId, string key, object value);
    public Task NotififyUsers(IEnumerable<string> userIds, string key, object value);
    public Task NotififyGroup(string groupId, string key, object value);

}
