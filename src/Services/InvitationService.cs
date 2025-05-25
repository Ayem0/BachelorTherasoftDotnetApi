using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Utils;

namespace BachelorTherasoftDotnetApi.src.Services;

public class InvitationService : IInvitationService
{
    private readonly IInvitationRepository _invitationRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IUserRepository _userRepository;
    private readonly ISocketService _socket;
    private readonly IMapper _mapper;
    private readonly IRedisService _cache;
    private static readonly TimeSpan ttl = TimeSpan.FromMinutes(10);

    public InvitationService(
        IInvitationRepository invitationRepository,
        IWorkspaceRepository workspaceRepository,
        IMapper mapper,
        ISocketService socket,
        IUserRepository userRepository,
        IRedisService cache
    )
    {
        _invitationRepository = invitationRepository;
        _workspaceRepository = workspaceRepository;
        _mapper = mapper;
        _socket = socket;
        _userRepository = userRepository;
        _cache = cache;
    }

    // Contact
    public async Task<InvitationDto> CreateContactInvitationAsync(string senderId, string receiverEmail)
    {
        var sender = await _userRepository.GetJoinContactsAndBlockedUsersByIdAsync(senderId) ?? throw new NotFoundException("User", senderId);
        var receiver = await _userRepository.GetJoinContactsAndBlockedUsersByEmailAsync(receiverEmail) ?? throw new NotFoundException("User", receiverEmail);

        if (sender.Contacts.Any(x => x.Id == receiver.Id) || sender.BlockedUsers.Any(x => x.Id == receiver.Id) ||
            receiver.Contacts.Any(x => x.Id == sender.Id || receiver.BlockedUsers.Any(x => x.Id == sender.Id)))
            throw new BadRequestException("", "");

        var invitation = new Invitation(sender, receiver) { Sender = sender, Receiver = receiver };
        var created = await _invitationRepository.CreateAsync(invitation);
        var dto = _mapper.Map<InvitationDto>(created);

        await _socket.NotififyUsers([receiver.Id, sender.Id], "ContactInvitationCreated", dto);
        await _cache.DeleteAsync(CacheKeys.InvitationsReceived(invitation.ReceiverId), CacheKeys.InvitationsSent(invitation.SenderId));
        return dto;
    }

    public async Task AcceptContactInvitationAsync(string userId, string id)
    {
        var key = CacheKeys.Invitation(id);
        var invitation = await _cache.GetOrSetAsync(key, () => _invitationRepository.GetByIdJoinSenderAndReceiver(id), ttl)
            ?? throw new NotFoundException("Invitation", id);

        if (invitation.ReceiverId != userId)
            throw new ForbiddenException();

        invitation.Sender.Contacts.Add(invitation.Receiver);
        invitation.Receiver.Contacts.Add(invitation.Sender);
        await _userRepository.UpdateMultipleAsync([invitation.Sender, invitation.Receiver]);

        await _invitationRepository.DeleteAsync(invitation);
        await _cache.DeleteAsync(
            CacheKeys.UserContacts(invitation.SenderId),
            CacheKeys.UserContacts(invitation.ReceiverId),
            CacheKeys.InvitationsReceived(invitation.ReceiverId),
            CacheKeys.InvitationsSent(invitation.SenderId),
            key
        );

        await _socket.NotififyUser(invitation.SenderId, "ContactAdded", _mapper.Map<UserDto>(invitation.Receiver));
        await _socket.NotififyUser(invitation.ReceiverId, "ContactAdded", _mapper.Map<UserDto>(invitation.Sender));
        await _socket.NotififyUsers([invitation.ReceiverId, invitation.SenderId], "ContactInvitationDeleted", invitation.Id);
    }

    public async Task CancelContactInvitationAsync(string userId, string id)
    {
        var key = CacheKeys.Invitation(id);
        var invitation = await _cache.GetOrSetAsync(key, () => _invitationRepository.GetByIdAsync(id), ttl)
            ?? throw new NotFoundException("Invitation", id);

        if (invitation.SenderId != userId)
            throw new ForbiddenException();

        await _invitationRepository.DeleteAsync(invitation);
        await _cache.DeleteAsync(
            CacheKeys.InvitationsReceived(invitation.ReceiverId),
            CacheKeys.InvitationsSent(invitation.SenderId),
            key
        );
        await _socket.NotififyUsers([invitation.ReceiverId, invitation.SenderId], "ContactInvitationDeleted", invitation.Id);
    }

    public async Task RefuseContactInvitationAsync(string userId, string id)
    {
        var key = CacheKeys.Invitation(id);
        var invitation = await _cache.GetOrSetAsync(key, () => _invitationRepository.GetByIdAsync(id), ttl)
            ?? throw new NotFoundException("Invitation", id);

        if (invitation.ReceiverId != userId)
            throw new ForbiddenException();

        await _invitationRepository.DeleteAsync(invitation);
        await _cache.DeleteAsync(
            CacheKeys.InvitationsReceived(invitation.ReceiverId),
            CacheKeys.InvitationsSent(invitation.SenderId),
            key
        );
        await _socket.NotififyUsers([invitation.ReceiverId, invitation.SenderId], "ContactInvitationDeleted", invitation.Id);
    }

    // Workspace
    public async Task<InvitationDto> CreateWorkspaceInvitationAsync(string senderUserId, CreateWorkspaceInvitationRequest req)
    {
        var sender = await _cache.GetOrSetAsync(CacheKeys.User(senderUserId), () => _userRepository.GetByIdAsync(senderUserId), ttl)
            ?? throw new NotFoundException("User", senderUserId);
        var receiver = await _cache.GetOrSetAsync(CacheKeys.User(req.ReceiverUserId), () => _userRepository.GetByIdAsync(req.ReceiverUserId), ttl)
            ?? throw new NotFoundException("User", req.ReceiverUserId);
        var workspace = await _cache.GetOrSetAsync(CacheKeys.Workspace(req.WorkspaceId), () => _workspaceRepository.GetByIdAsync(req.WorkspaceId), ttl)
            ?? throw new NotFoundException("Worspace", req.WorkspaceId);
        var users = await _cache.GetOrSetAsync(CacheKeys.WorkspaceUsers(req.WorkspaceId), () => _userRepository.GetByWorkspaceIdAsync(req.WorkspaceId), ttl);
        if (users.Any(u => u.Id == receiver.Id))
            throw new BadRequestException("User is already a member of this workspace.", "User is already a member of this workspace.");

        var existingInvitation = await _cache.GetOrSetAsync(
            CacheKeys.WorkspaceInvitation(req.WorkspaceId, req.ReceiverUserId),
            () => _invitationRepository.GetByWorkspaceIdAndReceiverId(req.WorkspaceId, req.ReceiverUserId),
            ttl
        );
        if (existingInvitation != null)
            throw new BadRequestException("", "");

        var invitation = new Invitation(workspace, sender, receiver) { Receiver = receiver, Sender = sender };
        var created = await _invitationRepository.CreateAsync(invitation);
        var dto = _mapper.Map<InvitationDto>(created);

        await _socket.NotififyGroup(workspace.Id, "WorkspaceInvitationCreated", dto);
        await _socket.NotififyUser(receiver.Id, "WorkspaceInvitationCreated", dto);
        await _cache.DeleteAsync(CacheKeys.WorkspaceInvitations(req.WorkspaceId), CacheKeys.InvitationsReceived(created.ReceiverId));

        return dto;
    }

    public async Task AcceptWorkspaceInvitationAsync(string userId, string id)
    {
        var key = CacheKeys.Invitation(id);
        var invitation = await _cache.GetOrSetAsync(key, () => _invitationRepository.GetByIdJoinWorkspaceAndReceiver(id), ttl)
            ?? throw new NotFoundException("Invitation", id);

        if (invitation.ReceiverId != userId || invitation.Workspace == null)
            throw new ForbiddenException();

        invitation.Workspace.Users.Add(invitation.Receiver);
        await _workspaceRepository.UpdateAsync(invitation.Workspace);
        await _invitationRepository.DeleteAsync(invitation);

        await _socket.NotififyGroup(invitation.Workspace.Id, "WorkspaceUserAdded", new { workspaceId = invitation.Workspace.Id, user = _mapper.Map<UserDto>(invitation.Receiver) });
        await _socket.NotififyUser(invitation.Receiver.Id, "WorkspaceAdded", _mapper.Map<WorkspaceDto>(invitation.Workspace));
        await _socket.NotififyGroup(invitation.Workspace.Id, "WorkspaceInvitationDeleted", invitation.Id);
        await _cache.DeleteAsync(
            CacheKeys.WorkspaceUsers(invitation.Workspace.Id),
            CacheKeys.UserWorkspaces(userId),
            CacheKeys.WorkspaceInvitations(invitation.Workspace.Id),
            CacheKeys.InvitationsReceived(invitation.ReceiverId),
            key
        );
    }

    public async Task CancelWorkspaceInvitationAsync(string userId, string id)
    {
        var key = CacheKeys.Invitation(id);
        var invitation = await _cache.GetOrSetAsync(key, () => _invitationRepository.GetByIdAsync(id), ttl)
            ?? throw new NotFoundException("Invitation", id);

        if (invitation.WorkspaceId == null)
            throw new ForbiddenException();

        var workspace = await _cache.GetOrSetAsync(CacheKeys.Workspace(invitation.WorkspaceId), () => _workspaceRepository.GetByIdAsync(invitation.WorkspaceId), ttl)
            ?? throw new NotFoundException("Worspace", invitation.WorkspaceId);
        var users = await _cache.GetOrSetAsync(CacheKeys.WorkspaceUsers(invitation.WorkspaceId), () => _userRepository.GetByWorkspaceIdAsync(invitation.WorkspaceId), ttl);

        if (!users.Any(x => x.Id == userId))
            throw new ForbiddenException();

        await _invitationRepository.DeleteAsync(invitation);
        await _socket.NotififyUser(invitation.ReceiverId, "WorkspaceInvitationDeleted", invitation.Id);
        await _socket.NotififyGroup(workspace.Id, "WorkspaceInvitationDeleted", invitation.Id);
        await _cache.DeleteAsync(CacheKeys.WorkspaceInvitations(invitation.WorkspaceId), CacheKeys.InvitationsReceived(invitation.ReceiverId), key);
    }

    public async Task RefuseWorkspaceInvitationAsync(string userId, string id)
    {
        var key = CacheKeys.Invitation(id);
        var invitation = await _cache.GetOrSetAsync(key, () => _invitationRepository.GetByIdAsync(id), ttl)
            ?? throw new NotFoundException("Invitation", id);

        if (invitation.ReceiverId != userId || invitation.WorkspaceId == null)
            throw new ForbiddenException();

        await _invitationRepository.DeleteAsync(invitation);
        await _socket.NotififyUser(invitation.ReceiverId, "WorkspaceInvitationDeleted", invitation.Id);
        await _socket.NotififyGroup(invitation.WorkspaceId, "WorkspaceInvitationDeleted", invitation.Id);
        await _cache.DeleteAsync(CacheKeys.WorkspaceInvitations(invitation.WorkspaceId), CacheKeys.InvitationsReceived(invitation.ReceiverId), key);
    }

    public Task<List<InvitationDto>> GetByReceiverUserIdAsync(string userId)
    => _cache.GetOrSetAsync<List<Invitation>, List<InvitationDto>>(
        CacheKeys.InvitationsReceived(userId),
        () => _invitationRepository.GetByReceiverUserIdAsync(userId),
        ttl
    );

    public Task<List<InvitationDto>> GetBySenderUserIdAsync(string userId)
    => _cache.GetOrSetAsync<List<Invitation>, List<InvitationDto>>(
        CacheKeys.InvitationsSent(userId),
        () => _invitationRepository.GetBySenderUserIdAsync(userId),
        ttl
    );

    public Task<List<InvitationDto>> GetByWorkspaceIdAsync(string workspaceId)
    => _cache.GetOrSetAsync<List<Invitation>, List<InvitationDto>>(
        CacheKeys.WorkspaceInvitations(workspaceId),
        () => _invitationRepository.GetByWorkspaceIdJoinSenderAndReceiverAsync(workspaceId),
        ttl
    );
}
