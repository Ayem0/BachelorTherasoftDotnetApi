using System;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Repositories;

public interface IInvitationRepository
{
    Task<List<Invitation>> GetByReceiverUserIdAsync(string userId);
    Task<Invitation?> GetByIdAsync(string id);
    Task<Invitation> UpdateAsync(Invitation invitation);
    Task<Invitation> CreateAsync(Invitation invitation);
    Task<bool> DeleteAsync(string id);
    Task<Invitation?> GetByWorkspaceIdAndReceiverId(string workspaceId, string receiverId);
    Task<Invitation?> GetByIdJoinWorkspaceAndReceiver(string id);
    Task<Invitation?> GetByIdJoinSenderAndReceiver(string id);
    Task<List<Invitation>> GetBySenderUserIdAsync(string userId);
    Task<List<Invitation>> GetByWorkspaceIdJoinWorkspaceMembersAsync(string workspaceId);
}
