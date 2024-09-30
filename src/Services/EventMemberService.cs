using System;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;

namespace BachelorTherasoftDotnetApi.src.Services;

public class EventMemberService
{
    private readonly IEventMemberRepository _eventMemberRepository;
    public EventMemberService(IEventMemberRepository eventMemberRepository)
    {
        _eventMemberRepository = eventMemberRepository;
    }

    
}
