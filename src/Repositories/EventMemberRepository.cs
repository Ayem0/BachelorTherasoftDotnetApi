using System;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class EventMemberRepository : BaseMySqlRepository<EventMember>, IEventMemberRepository
{
    public EventMemberRepository(MySqlDbContext context) : base(context)
    {
        
    }
}
