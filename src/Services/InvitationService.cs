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
    private readonly UserManager<User> _userManager;
    private readonly IHubContext<WorkspaceHub> _hubContext;
    private readonly IMapper _mapper;
    public InvitationService(
        IInvitationRepository invitationRepository,
        IWorkspaceRepository workspaceRepository,
        IEventRepository eventRepository,
        UserManager<User> userManager,
        IMapper mapper,
        IHubContext<WorkspaceHub> hubContext,
        IUserRepository userRepository
    )
    {
        _invitationRepository = invitationRepository;
        _workspaceRepository = workspaceRepository;
        _eventRepository = eventRepository;
        _mapper = mapper;
        _userManager = userManager;
        _hubContext = hubContext;
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
        await _hubContext.Clients.User(receiver.Id).SendAsync("ContactInvitationReceived", invitation);
        return _mapper.Map<InvitationDto>(invitation);
    }

    public async Task AcceptContactInvitationAsync(string userId, string id)
    {
        var invitation = await _invitationRepository.GetByIdJoinSenderAndReceiver(id) ?? throw new NotFoundException("Invitation", id);
        if (invitation.ReceiverId != userId) throw new ForbiddenException();
        invitation.Sender.Contacts.Add(invitation.Receiver);
        invitation.Receiver.Contacts.Add(invitation.Sender);
        await _userRepository.UpdateMultipleAsync([invitation.Sender, invitation.Receiver]);
        await _invitationRepository.DeleteAsync(invitation.Id);
        await _hubContext.Clients.User(invitation.SenderId).SendAsync("ContactInvitationAccepted", _mapper.Map<UserDto>(invitation.Receiver));
        await _hubContext.Clients.User(invitation.ReceiverId).SendAsync("ContactAdded", _mapper.Map<UserDto>(invitation.Sender));
    }

    public async Task CancelContactInvitationAsync(string userId, string id)
    {
        var invitation = await _invitationRepository.GetByIdAsync(id) ?? throw new NotFoundException("Invitation", id);
        if (invitation.SenderId != userId) throw new ForbiddenException();
        await _hubContext.Clients.User(invitation.ReceiverId).SendAsync("ContactInvitationCanceled", invitation.Id);
        await _invitationRepository.DeleteAsync(id);
    }

    public async Task RefuseContactInvitationAsync(string userId, string id)
    {
        var invitation = await _invitationRepository.GetByIdAsync(id) ?? throw new NotFoundException("Invitation", id);
        if (invitation.ReceiverId != userId) throw new ForbiddenException();
        await _invitationRepository.DeleteAsync(id);
        await _hubContext.Clients.Client(invitation.SenderId).SendAsync("ContactInvitationRefused", invitation.Id);
    }

    // Workspace
    public async Task<InvitationDto> CreateWorkspaceInvitationAsync(string senderUserId, CreateWorkspaceInvitationRequest req)
    {
        var sender = await _userManager.FindByIdAsync(senderUserId) ?? throw new NotFoundException("User", senderUserId);
        var receiver = await _userManager.FindByIdAsync(req.ReceiverUserId) ?? throw new NotFoundException("User", req.ReceiverUserId);
        var workspace = await _workspaceRepository.GetByIdAsync(req.WorkspaceId) ?? throw new NotFoundException("Workspace", req.WorkspaceId);
        var existingInvitation = await _invitationRepository.GetByWorkspaceIdAndReceiverId(req.WorkspaceId, req.ReceiverUserId);
        if (existingInvitation != null) throw new BadRequestException("", "");
        var invitation = await _invitationRepository.CreateAsync(new Invitation(workspace, sender, receiver) { Receiver = receiver, Sender = sender });
        var invitationDto = _mapper.Map<InvitationDto>(invitation);
        await _hubContext.Clients.User(invitation.ReceiverId).SendAsync("WorkspaceInvitationCreated", invitationDto);
        return invitationDto;
    }

    public async Task AcceptWorkspaceInvitationAsync(string userId, string id)
    {
        var invitation = await _invitationRepository.GetByIdJoinWorkspaceAndReceiver(id) ?? throw new NotFoundException("Invitation", id);
        if (invitation.ReceiverId != userId || invitation.Workspace == null) throw new ForbiddenException();
        invitation.Workspace.Users.Add(invitation.Receiver);
        await _workspaceRepository.UpdateAsync(invitation.Workspace);
        await _hubContext.Clients.Group(invitation.Workspace.Id).SendAsync("WorkspaceMemberAdded", _mapper.Map<UserDto>(invitation.Receiver));
        await _invitationRepository.DeleteAsync(invitation.Id);
    }

    public async Task CancelWorkspaceInvitationAsync(string userId, string id)
    {
        var invitation = await _invitationRepository.GetByIdAsync(id) ?? throw new NotFoundException("Invitation", id);
        if (invitation.WorkspaceId == null) throw new BadRequestException("Bad request", ""); // TODO comment nommer l'erreur
        var workspace = await _workspaceRepository.GetJoinUsersByIdAsync(invitation.WorkspaceId) ?? throw new NotFoundException("Workspace", id);
        if (!workspace.Users.Any(x => x.Id == userId)) throw new ForbiddenException();
        await _hubContext.Clients.User(invitation.ReceiverId).SendAsync("WorkspaceInvitationDeleted", id);
        await _invitationRepository.DeleteAsync(id);
        await _hubContext.Clients.Group(invitation.WorkspaceId).SendAsync("WorkspaceInvitationDeleted", invitation.Id);
    }

    public async Task RefuseWorkspaceInvitationAsync(string userId, string id)
    {
        var invitation = await _invitationRepository.GetByIdAsync(id) ?? throw new NotFoundException("Invitation", id);
        if (invitation.WorkspaceId == null) throw new BadRequestException("Bad request", ""); // TODO comment nommer l'erreur
        if (invitation.ReceiverId != userId) throw new ForbiddenException();
        await _invitationRepository.DeleteAsync(id);
        await _hubContext.Clients.Group(invitation.WorkspaceId).SendAsync("WorkspaceInvitationDeleted", invitation.Id);
    }

    /// <summary>Get all user received invitations</summary>
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
        if (!user.Workspaces.Any(w => w.Id == workspaceId)) throw new ForbiddenException();
        var invitations = await _invitationRepository.GetByWorkspaceIdJoinWorkspaceMembersAsync(workspaceId);

        return [.. invitations.Select(_mapper.Map<InvitationDto>)];
    }
}
