using System;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class EventMemberRepository : BaseMySqlRepository<EventMember>, IEventMemberRepository
{
    public EventMemberRepository(MySqlDbContext context) : base(context)
    {
        
    }

    public Task<EventMember?> GetByEventMemberIds(string eventId, string memberId)
    {
        return _context.EventMember.Where(x => x.EventId == eventId && x.MemberId == memberId).FirstOrDefaultAsync();
    }

    public Task<EventMember?> GetByUserEventIds(string userId, string eventId)
    {
        return _context.EventMember
            .Include(x => x.Member)
            .Where(x => x.EventId == eventId && x.Member.UserId == userId).FirstOrDefaultAsync();
    }
}
