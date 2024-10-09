using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
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

    public async Task<Response<EventMemberDto?>> CreateAsync(string eventId, string memberId)
    {

        var existingEventMember = await _eventMemberRepository.GetByEventMemberIds(eventId, memberId);
        if (existingEventMember != null) return new Response<EventMemberDto?>(success: false, errors: ["Event already contains this member."]);

        var eventToGet = await _eventRepository.GetByIdJoinWorkspaceAsync(eventId);
        if(eventToGet == null) return new Response<EventMemberDto?>(success: false, errors: ["Event not found."]);

        var member = await _memberRepository.GetByIdAsync(memberId);
        if(member == null) return new Response<EventMemberDto?>(success: false, errors: ["Member not found."]);

        if (eventToGet.Room.Area.Location.WorkspaceId != member.WorkspaceId) 
            return new Response<EventMemberDto?>(success: false, errors: ["Workspace does not contain this member."]);
            
        var eventMember = new EventMember(member, eventToGet) {
            Member = member,
            Event = eventToGet
        };        
        await _eventMemberRepository.CreateAsync(eventMember);

        return new Response<EventMemberDto?>(success: true, content: new EventMemberDto(eventMember));
    }

    public async Task<Response<string>> DeleteAsync(string id)
    {
        var eventToDelete = await _eventMemberRepository.GetByIdAsync(id);
        if(eventToDelete == null) return new Response<string>(success: false, errors: ["Event not found."]);

        await _eventMemberRepository.DeleteAsync(eventToDelete);
        return new Response<string>(success: true, content: "Event deleted successfully.");
    }

    public async Task<Response<EventMemberDto?>> GetByIdAsync(string id)
    {
        var eventToGet = await _eventMemberRepository.GetByIdAsync(id);

        if (eventToGet == null) return new Response<EventMemberDto?>(success: false, errors: ["Event not found."]); 

        return new Response<EventMemberDto?>( success: true, content: new EventMemberDto(eventToGet));
    }

    public async Task<Response<EventMemberDto?>> UpdateAsync(string id, Status? newStatus)
    {
        var eventToUpdate = await _eventMemberRepository.GetByIdAsync(id);
        if(eventToUpdate == null || (newStatus == null)) return new Response<EventMemberDto?>(success: false, errors: ["Event not found."]); 

        eventToUpdate.Status = newStatus ?? eventToUpdate.Status;

        await _eventMemberRepository.UpdateAsync(eventToUpdate);
        return new Response<EventMemberDto?>(success: true, content: new EventMemberDto(eventToUpdate)); 
    }
}
