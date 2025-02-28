using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IInvitationService
{
    Task<InvitationDto> CreateWorkspaceInvitationAsync(string senderId, CreateWorkspaceInvitationRequest req);
    Task AcceptWorkspaceInvitationAsync(string userId, string id);
    Task CancelWorkspaceInvitationAsync(string userId, string id);
    Task RefuseWorkspaceInvitationAsync(string userId, string id);
    Task<InvitationDto> CreateContactInvitationAsync(string senderId, string receiverEmail);
    Task AcceptContactInvitationAsync(string userId, string id);
    Task<List<InvitationDto>> GetByReceiverUserIdAsync(string userId);
    Task<List<InvitationDto>> GetBySenderUserIdAsync(string userId);
    Task<List<InvitationDto>> GetByWorkspaceIdAsync(string userId, string workspaceId);
    Task CancelContactInvitationAsync(string userId, string id);
    Task RefuseContactInvitationAsync(string userId, string id);


}
