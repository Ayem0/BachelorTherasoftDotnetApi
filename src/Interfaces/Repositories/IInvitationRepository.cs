using System;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Repositories;

public interface IInvitationRepository : IBaseRepository<Invitation>
{
    Task<List<Invitation>> GetByReceiverUserIdAsync(string userId);
    Task<Invitation?> GetByWorkspaceIdAndReceiverId(string workspaceId, string receiverId);
    Task<Invitation?> GetByIdJoinWorkspaceAndReceiver(string id);
    Task<Invitation?> GetByIdJoinSenderAndReceiver(string id);
    Task<List<Invitation>> GetBySenderUserIdAsync(string userId);
    Task<List<Invitation>> GetByWorkspaceIdJoinSenderAndReceiverAsync(string workspaceId);
}
