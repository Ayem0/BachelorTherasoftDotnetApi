using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Services;

public class InvitationService : IInvitationService
{
    private readonly IInvitationRepository _invitationRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IEventMemberRepository _eventMemberRepository;
    private readonly IMemberRepository _memberRepository;
    public InvitationService(IInvitationRepository invitationRepository, IWorkspaceRepository workspaceRepository, IEventRepository eventRepository, IEventMemberRepository eventMemberRepository, IMemberRepository memberRepository)
    {
        _invitationRepository = invitationRepository;
        _workspaceRepository = workspaceRepository;
        _eventRepository = eventRepository;
        _eventMemberRepository = eventMemberRepository;
        _memberRepository = memberRepository;
    }
    public async Task<IActionResult> AcceptAsync(string id, User user)
    {
        var invitation = await _invitationRepository.GetByIdAsync(id);
        if (invitation == null) return new NotFoundObjectResult("Invitation not found.");

        if (invitation.ReceiverUserId != user.Id) return new UnauthorizedResult();

        if (invitation.InvitationType == InvitationType.Workspace && invitation.WorkspaceId != null)
        {
            var workspace = await _workspaceRepository.GetByIdAsync(invitation.WorkspaceId);
            if (workspace == null) return new NotFoundObjectResult("Workspace not found.");

            var member = new Member(user, workspace) { User = user, Workspace = workspace };

            await _memberRepository.CreateAsync(member);
            await _invitationRepository.DeleteAsync(invitation);

            return new OkObjectResult("Successfully accepted invitation.");
        }
        else if (invitation.InvitationType == InvitationType.Event && invitation.EventId != null)
        {
            var @event = await _eventRepository.GetByIdAsync(invitation.EventId);
            if (@event == null) return new NotFoundObjectResult("Event not found.");

            var eventMember = await _eventMemberRepository.GetByUserEventIds(user.Id, @event.Id);
            if (eventMember == null) return new NotFoundObjectResult("Event does not contain this member.");

            eventMember.Status = Status.Accepted;
            await _eventMemberRepository.UpdateAsync(eventMember);
            await _invitationRepository.DeleteAsync(invitation);

            return new OkObjectResult("Successfully accepted invitation.");
        }
        return new BadRequestObjectResult("Workspace or event must be provided.");
    }
   
    // TODO
    public async Task<IActionResult> CancelAsync(string id, User user)
    {
        var invitation = await _invitationRepository.GetByIdAsync(id);
        if (invitation == null) return new NotFoundObjectResult("Invitation not found.");

        if (invitation.SenderUserId != user.Id) return new UnauthorizedResult();

        if (invitation.InvitationType == InvitationType.Event) {
            if (invitation.EventId == null) return new BadRequestResult();

            var eventMember = await _eventMemberRepository.GetByUserEventIds(user.Id, invitation.EventId);
            if (eventMember == null) return new NotFoundObjectResult("Member not found.");

            await _eventMemberRepository.DeleteAsync(eventMember);
        }
        await _invitationRepository.DeleteAsync(invitation);

        return new OkObjectResult("Successfully canceled invitation.");
    }
    // TODO
    public Task<IActionResult> CreateEventInvitationAsync(string senderId, string receiverId, string eventId)
    {
        throw new NotImplementedException();
    }
    // TODO
    public Task<IActionResult> CreateWorkspaceInvitationAsync(string senderId, string receiverId, string workspaceId)
    {
        throw new NotImplementedException();
    }

    // public async Task<IActionResult> GetByIdAsync(string id)
    // {
    //     var invitation = await _invitationRepository.GetByIdAsync(id);
    //     if (invitation == null) return new NotFoundObjectResult("Invitation not found.");

    //     return new OkObjectResult(new InvitationDto(invitation));
    // }

    public async Task<IActionResult> GetByReceiverUserAsync(User user)
    {
        var invitations = await _invitationRepository.GetByReceiverUserIdAsync(user.Id);

        return new OkObjectResult(invitations.Select(x => new InvitationDto(x)).ToList());
    }
}
