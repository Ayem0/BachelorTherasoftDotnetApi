using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IInvitationService
{
    Task<bool> CancelAsync(string id, User user);
    Task<bool> AcceptAsync(string id, User user);
    Task<InvitationDto> CreateEventInvitationAsync(string senderId, string receiverId, string eventId);
    Task<InvitationDto> CreateWorkspaceInvitationAsync(string senderId, string receiverId, string workspaceId);
    Task<List<InvitationDto>> GetByReceiverUserAsync(User user);

}
