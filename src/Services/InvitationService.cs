using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.AspNetCore.Identity;

namespace BachelorTherasoftDotnetApi.src.Services;

// TODO ajouter le websocketservice qui n'existe pas encore
public class InvitationService : IInvitationService
{
    private readonly IInvitationRepository _invitationRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IEventRepository _eventRepository;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    public InvitationService(IInvitationRepository invitationRepository, IWorkspaceRepository workspaceRepository, IEventRepository eventRepository, UserManager<User> userManager, IMapper mapper)
    {
        _invitationRepository = invitationRepository;
        _workspaceRepository = workspaceRepository;
        _eventRepository = eventRepository;
        _mapper = mapper;
        _userManager = userManager;
    }
    public async Task<bool> AcceptAsync(string id, User user)
    {
        var invitation = await _invitationRepository.GetEntityByIdAsync(id) ?? throw new NotFoundException("Location", id);
        if (invitation.ReceiverId != user.Id) return false;

        if (invitation.InvitationType == InvitationType.Workspace && invitation.Workspace != null)
        {
            invitation.Workspace.Users.Add(user); // TODO A REFACTOR LES 2 REQUETES DE DESSOUS

            await _workspaceRepository.UpdateAsync(invitation.Workspace);

            return await _invitationRepository.DeleteAsync(invitation.Id);
        }
        else if (invitation.InvitationType == InvitationType.Event && invitation.Event != null)
        {
            var eventUser = invitation.Event.Users.Where(x => x.UserId == user.Id).FirstOrDefault();
            if (eventUser == null) return false;

            eventUser.Status = Status.Accepted;
            await _eventRepository.UpdateAsync(invitation.Event);
            return await _invitationRepository.DeleteAsync(invitation.Id); // TODO voir si cela la delete sans cette ligne
        }
        return false;
        
    }

    public async Task<bool> CancelAsync(string id, User user)
    {
        var invitation = await _invitationRepository.GetEntityByIdAsync(id) ?? throw new NotFoundException("Invitation", id);
        if (invitation.SenderId != user.Id) return false; // TODO a changer par le droit

        if (invitation.InvitationType == InvitationType.Event && invitation.Event != null)
        {
            var eventUser = invitation.Event.Users.Where(x => x.UserId == user.Id && x.DeletedAt == null).FirstOrDefault();
            if (eventUser == null) return false;

            await _eventRepository.UpdateAsync(invitation.Event);
        }
        var res = await _invitationRepository.DeleteAsync(id);

        return true;
    }

    public async Task<InvitationDto> CreateEventInvitationAsync(string senderId, string receiverId, string eventId)
    {
        var sender = await _userManager.FindByIdAsync(senderId) ?? throw new NotFoundException("User", senderId);

        var receiver = await _userManager.FindByIdAsync(receiverId) ?? throw new NotFoundException("User", senderId);

        var @event = await _eventRepository.GetEntityByIdAsync(eventId) ?? throw new NotFoundException("User", senderId);

        var existingInvitation = await _invitationRepository.GetEntityByIdAsync(eventId + "_" + receiverId);
        if (existingInvitation != null) throw new NotFoundException("Invitation", senderId); // TODO a changer

        var invitation = new Invitation(@event, sender, receiver) { Receiver = receiver, Sender = sender };

        await _invitationRepository.CreateAsync(invitation);

        return _mapper.Map<InvitationDto>(invitation);
    }

    public async Task<InvitationDto> CreateWorkspaceInvitationAsync(string senderId, string receiverId, string workspaceId)
    {
        var sender = await _userManager.FindByIdAsync(senderId) ?? throw new NotFoundException("User", senderId);

        var receiver = await _userManager.FindByIdAsync(receiverId) ?? throw new NotFoundException("User", receiverId);

        var workspace = await _workspaceRepository.GetByIdAsync(workspaceId) ?? throw new NotFoundException("Workspace", workspaceId);

        var existingInvitation = await _invitationRepository.GetEntityByIdAsync(workspaceId + "_" + receiverId);
        if (existingInvitation != null) throw new NotFoundException("Invitation", senderId); // TODO a changer

        var invitation = new Invitation(workspace, sender, receiver) { Receiver = receiver, Sender = sender };
        await _invitationRepository.CreateAsync(invitation);

        return _mapper.Map<InvitationDto>(invitation);
    }

    // public async Task<IActionResult> GetByIdAsync(string id)
    // {
    //     var invitation = await _invitationRepository.GetByIdAsync(id);
    //     if (invitation == null) return Response.NotFound(id, "Invitation");

    //     return Response.Ok(new InvitationDto(invitation));
    // }

    public async Task<List<InvitationDto>> GetByReceiverUserAsync(User user)
    {
        var invitations = await _invitationRepository.GetByReceiverUserIdAsync(user.Id);

        return invitations.Select(x => _mapper.Map<InvitationDto>(x)).ToList();
    }
}
