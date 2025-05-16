using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Hubs;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace BachelorTherasoftDotnetApi.src.Services;

public class InvitationService : IInvitationService
{
    private readonly IInvitationRepository _invitationRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IHubService _hub;
    private readonly IMapper _mapper;
    public InvitationService(
        IInvitationRepository invitationRepository,
        IWorkspaceRepository workspaceRepository,
        IEventRepository eventRepository,
        IMapper mapper,
        IHubService hub,
        IUserRepository userRepository
    )
    {
        _invitationRepository = invitationRepository;
        _workspaceRepository = workspaceRepository;
        _eventRepository = eventRepository;
        _mapper = mapper;
        _hub = hub;
        _userRepository = userRepository;
    }

    // Contact
    public async Task<InvitationDto> CreateContactInvitationAsync(string senderId, string receiverEmail)
    {
        var sender = await _userRepository.GetByIdJoinContactsAndBlockedUsersAsync(senderId) ?? throw new NotFoundException("User", senderId);
        var receiver = await _userRepository.GetByEmailJoinContactsAndBlockedUsersAsync(receiverEmail) ?? throw new NotFoundException("User", receiverEmail);

        if (sender.Contacts.Any(x => x.Id == receiver.Id) || sender.BlockedUsers.Any(x => x.Id == receiver.Id) ||
            receiver.Contacts.Any(x => x.Id == sender.Id || receiver.BlockedUsers.Any(x => x.Id == sender.Id)))
            throw new BadRequestException("", "");

        var invitation = await _invitationRepository.CreateAsync(new Invitation(sender, receiver) { Sender = sender, Receiver = receiver });
        var invitationDto = _mapper.Map<InvitationDto>(invitation);

        await _hub.NotififyUsers([receiver.Id, sender.Id], "ContactInvitationCreated", invitation);

        return _mapper.Map<InvitationDto>(invitation);
    }

    public async Task AcceptContactInvitationAsync(string userId, string id)
    {
        var invitation = await _invitationRepository.GetByIdJoinSenderAndReceiver(id) ?? throw new NotFoundException("Invitation", id);

        if (invitation.ReceiverId != userId)
            throw new ForbiddenException();

        invitation.Sender.Contacts.Add(invitation.Receiver);
        invitation.Receiver.Contacts.Add(invitation.Sender);
        await _userRepository.UpdateMultipleAsync([invitation.Sender, invitation.Receiver]);
        await _invitationRepository.DeleteAsync(invitation.Id);

        await _hub.NotififyUser(invitation.SenderId, "ContactAdded", _mapper.Map<UserDto>(invitation.Receiver));
        await _hub.NotififyUser(invitation.ReceiverId, "ContactAdded", _mapper.Map<UserDto>(invitation.Sender));
        await _hub.NotififyUsers([invitation.ReceiverId, invitation.SenderId], "ContactInvitationDeleted", invitation.Id);
    }

    public async Task CancelContactInvitationAsync(string userId, string id)
    {
        var invitation = await _invitationRepository.GetByIdAsync(id) ?? throw new NotFoundException("Invitation", id);

        if (invitation.SenderId != userId)
            throw new ForbiddenException();

        await _invitationRepository.DeleteAsync(id);
        await _hub.NotififyUsers([invitation.ReceiverId, invitation.SenderId], "ContactInvitationDeleted", invitation.Id);
    }

    public async Task RefuseContactInvitationAsync(string userId, string id)
    {
        var invitation = await _invitationRepository.GetByIdAsync(id) ?? throw new NotFoundException("Invitation", id);

        if (invitation.ReceiverId != userId)
            throw new ForbiddenException();

        await _invitationRepository.DeleteAsync(id);
        await _hub.NotififyUsers([invitation.ReceiverId, invitation.SenderId], "ContactInvitationDeleted", invitation.Id);
    }

    // Workspace
    public async Task<InvitationDto> CreateWorkspaceInvitationAsync(string senderUserId, CreateWorkspaceInvitationRequest req)
    {
        var sender = await _userRepository.GetByIdAsync(senderUserId) ?? throw new NotFoundException("User", senderUserId);
        var receiver = await _userRepository.GetByIdAsync(req.ReceiverUserId) ?? throw new NotFoundException("User", req.ReceiverUserId);
        var workspace = await _workspaceRepository.GetJoinUsersByIdAsync(req.WorkspaceId) ?? throw new NotFoundException("Workspace", req.WorkspaceId);

        if (workspace.Users.Any(u => u.Id == receiver.Id))
            throw new BadRequestException("User is already a member of this workspace.", "User is already a member of this workspace.");

        var existingInvitation = await _invitationRepository.GetByWorkspaceIdAndReceiverId(req.WorkspaceId, req.ReceiverUserId);

        if (existingInvitation != null)
            throw new BadRequestException("", "");

        var invitation = await _invitationRepository.CreateAsync(new Invitation(workspace, sender, receiver) { Receiver = receiver, Sender = sender });
        var invitationDto = _mapper.Map<InvitationDto>(invitation);
        await _hub.NotififyGroup(workspace.Id, "WorkspaceInvitationCreated", invitationDto);
        await _hub.NotififyUser(receiver.Id, "WorkspaceInvitationCreated", invitationDto);

        return invitationDto;
    }

    public async Task AcceptWorkspaceInvitationAsync(string userId, string id)
    {
        var invitation = await _invitationRepository.GetByIdJoinWorkspaceAndReceiver(id) ?? throw new NotFoundException("Invitation", id);

        if (invitation.ReceiverId != userId || invitation.Workspace == null)
            throw new ForbiddenException();

        invitation.Workspace.Users.Add(invitation.Receiver);
        await _workspaceRepository.UpdateAsync(invitation.Workspace);
        await _hub.NotififyGroup(invitation.Workspace.Id, "WorkspaceUserAdded", new { workspaceId = invitation.Workspace.Id, user = _mapper.Map<UserDto>(invitation.Receiver) });
        await _hub.NotififyUser(invitation.Receiver.Id, "WorkspaceAdded", _mapper.Map<WorkspaceDto>(invitation.Workspace));
        await _invitationRepository.DeleteAsync(invitation.Id);
        await _hub.NotififyGroup(invitation.Workspace.Id, "WorkspaceInvitationDeleted", invitation.Id);
    }

    public async Task CancelWorkspaceInvitationAsync(string userId, string id)
    {
        var invitation = await _invitationRepository.GetByIdAsync(id) ?? throw new NotFoundException("Invitation", id);

        if (invitation.WorkspaceId == null)
            throw new BadRequestException("Bad request", ""); // TODO comment nommer l'erreur

        var workspace = await _workspaceRepository.GetJoinUsersByIdAsync(invitation.WorkspaceId) ?? throw new NotFoundException("Workspace", id);

        if (!workspace.Users.Any(x => x.Id == userId))
            throw new ForbiddenException();

        await _invitationRepository.DeleteAsync(invitation.Id);
        await _hub.NotififyUser(invitation.ReceiverId, "WorkspaceInvitationDeleted", invitation.Id);
        await _hub.NotififyGroup(workspace.Id, "WorkspaceInvitationDeleted", invitation.Id);
    }

    public async Task RefuseWorkspaceInvitationAsync(string userId, string id)
    {
        var invitation = await _invitationRepository.GetByIdAsync(id) ?? throw new NotFoundException("Invitation", id);

        if (invitation.WorkspaceId == null)
            throw new BadRequestException("Bad request", ""); // TODO comment nommer l'erreur

        if (invitation.ReceiverId != userId)
            throw new ForbiddenException();

        await _invitationRepository.DeleteAsync(id);
        await _hub.NotififyUser(invitation.ReceiverId, "WorkspaceInvitationDeleted", invitation.Id);
        await _hub.NotififyGroup(invitation.WorkspaceId, "WorkspaceInvitationDeleted", invitation.Id);
    }

    public async Task<List<InvitationDto>> GetByReceiverUserIdAsync(string userId)
    {
        var invitations = await _invitationRepository.GetByReceiverUserIdAsync(userId);

        return [.. invitations.Select(_mapper.Map<InvitationDto>)];
    }

    public async Task<List<InvitationDto>> GetBySenderUserIdAsync(string userId)
    {
        var invitations = await _invitationRepository.GetBySenderUserIdAsync(userId);

        return [.. invitations.Select(_mapper.Map<InvitationDto>)];
    }

    public async Task<List<InvitationDto>> GetByWorkspaceIdAsync(string userId, string workspaceId)
    {
        var user = await _userRepository.GetByIdJoinWorkspaceAsync(userId) ?? throw new NotFoundException("User", userId);

        if (!user.Workspaces.Any(w => w.Id == workspaceId))
            throw new ForbiddenException();

        var invitations = await _invitationRepository.GetByWorkspaceIdJoinSenderAndReceiverAsync(workspaceId);

        return [.. invitations.Select(_mapper.Map<InvitationDto>)];
    }
}
