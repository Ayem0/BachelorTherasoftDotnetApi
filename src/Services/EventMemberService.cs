using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Services;

public class EventMemberService : IEventMemberService
{
    private readonly IEventMemberRepository _eventMemberRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IMemberRepository _memberRepository;
    public EventMemberService(IEventMemberRepository eventMemberRepository, IEventRepository eventRepository, IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
        _eventMemberRepository = eventMemberRepository;
        _eventRepository = eventRepository;
    }

    public async Task<EventMemberDto?> CreateAsync(string eventId, string memberId)
    {
        var eventToGet = await _eventRepository.GetByIdJoinWorkspaceAsync(eventId);
        if(eventToGet == null) return null;

        var member = await _memberRepository.GetByIdAsync(memberId);
        if(member == null) return null;

        if (eventToGet.Room.Area.Location.WorkspaceId != member.WorkspaceId || eventToGet.Members.Exists(x => x.MemberId == member.Id)) return null;

        var eventMember = new EventMember(member, eventToGet) {
            Member = member,
            Event = eventToGet
        };        
        await _eventMemberRepository.CreateAsync(eventMember);

        return new EventMemberDto(eventMember);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var eventToDelete = await _eventMemberRepository.GetByIdAsync(id);
        if(eventToDelete == null) return false;

        await _eventMemberRepository.DeleteAsync(eventToDelete);
        return true;
    }

    public async Task<EventMemberDto?> GetByIdAsync(string id)
    {
        var eventToGet = await _eventMemberRepository.GetByIdAsync(id);

        return eventToGet != null ? new EventMemberDto(eventToGet) : null;
    }

    public async Task<EventMemberDto?> UpdateAsync(string id, Status? newStatus)
    {
        var eventToUpdate = await _eventMemberRepository.GetByIdAsync(id);
        if(eventToUpdate == null || (newStatus == null)) return null;

        eventToUpdate.Status = newStatus ?? eventToUpdate.Status;

        await _eventMemberRepository.UpdateAsync(eventToUpdate);
        return new EventMemberDto(eventToUpdate);
    }
}
