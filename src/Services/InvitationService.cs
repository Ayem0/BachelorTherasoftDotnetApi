using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Services;

// TODO ajouter le websocketservice qui n'existe pas encore
public class InvitationService : IInvitationService
{
    private readonly IInvitationRepository _invitationRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IEventMemberRepository _eventMemberRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly UserManager<User> _userManager;
    public InvitationService(IInvitationRepository invitationRepository, IWorkspaceRepository workspaceRepository, IEventRepository eventRepository, 
        IEventMemberRepository eventMemberRepository, IMemberRepository memberRepository, UserManager<User> userManager)
    {
        _invitationRepository = invitationRepository;
        _workspaceRepository = workspaceRepository;
        _eventRepository = eventRepository;
        _eventMemberRepository = eventMemberRepository;
        _memberRepository = memberRepository;
        _userManager = userManager;
    }
    public async Task<ActionResult> AcceptAsync(string id, User user)
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
   
    public async Task<ActionResult> CancelAsync(string id, User user)
    {
        var invitation = await _invitationRepository.GetByIdAsync(id);
        if (invitation == null) return new NotFoundObjectResult("Invitation not found.");

        if (invitation.SenderUserId != user.Id) return new UnauthorizedResult(); // TODO a changer par le droit

        if (invitation.InvitationType == InvitationType.Event) {
            if (invitation.EventId == null) return new BadRequestResult();

            var eventMember = await _eventMemberRepository.GetByUserEventIds(user.Id, invitation.EventId);
            if (eventMember == null) return new NotFoundObjectResult("Member not found.");

            await _eventMemberRepository.DeleteAsync(eventMember);
        }
        await _invitationRepository.DeleteAsync(invitation);

        return new OkObjectResult("Successfully canceled invitation.");
    }
 
    public async Task<ActionResult<InvitationDto>> CreateEventInvitationAsync(string senderId, string receiverId, string eventId)
    {
        var sender = await _userManager.FindByIdAsync(senderId);
        if (sender == null) return new UnauthorizedResult();

        var receiver = await _userManager.FindByIdAsync(receiverId);
        if (receiver == null) return new NotFoundObjectResult("User not found.");

        var @event = await _eventRepository.GetByIdAsync(eventId);
        if (@event == null) return new NotFoundObjectResult("Workspace not found.");

        var existingInvitation = await _invitationRepository.GetByIdAsync(eventId + "_" + receiverId);
        if (existingInvitation == null) return new BadRequestObjectResult("User already invited.");

        var invitation = new Invitation(eventId + "_" + receiverId, InvitationType.Event, null, eventId, senderId, receiverId);
        await _invitationRepository.CreateAsync(invitation);

        return new CreatedAtActionResult(
            actionName: "Create", 
            controllerName: "Invitation", 
            routeValues: new { id = invitation.Id }, 
            value: new InvitationDto(invitation)
        );  
    }
 
    public async Task<ActionResult<InvitationDto>> CreateWorkspaceInvitationAsync(string senderId, string receiverId, string workspaceId)
    {
        var sender = await _userManager.FindByIdAsync(senderId);
        if (sender == null) return new UnauthorizedResult();

        var receiver = await _userManager.FindByIdAsync(receiverId);
        if (receiver == null) return new NotFoundObjectResult("User not found.");

        var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);
        if (workspace == null) return new NotFoundObjectResult("Workspace not found.");

        var existingInvitation = await _invitationRepository.GetByIdAsync(workspaceId + "_" + receiverId);
        if (existingInvitation == null) return new BadRequestObjectResult("User already invited.");

        var invitation = new Invitation(workspaceId + "_" + receiverId, InvitationType.Workspace, workspaceId, null, senderId, receiverId);
        await _invitationRepository.CreateAsync(invitation);

        return new CreatedAtActionResult(
            actionName: "Create", 
            controllerName: "Invitation", 
            routeValues: new { id = invitation.Id }, 
            value: new InvitationDto(invitation)
        );      
    }

    // public async Task<IActionResult> GetByIdAsync(string id)
    // {
    //     var invitation = await _invitationRepository.GetByIdAsync(id);
    //     if (invitation == null) return new NotFoundObjectResult("Invitation not found.");

    //     return new OkObjectResult(new InvitationDto(invitation));
    // }

    public async Task<ActionResult<List<InvitationDto>>> GetByReceiverUserAsync(User user)
    {
        var invitations = await _invitationRepository.GetByReceiverUserIdAsync(user.Id);

        return new OkObjectResult(invitations.Select(x => new InvitationDto(x)).ToList());
    }
}
