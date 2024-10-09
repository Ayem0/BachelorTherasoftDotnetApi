using System;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Repositories;

public interface IEventMemberRepository : IBaseRepository<EventMember>
{
    Task<EventMember?> GetByEventMemberIds(string eventId, string memberId);
    Task<EventMember?> GetByUserEventIds(string userId, string eventId);
}
