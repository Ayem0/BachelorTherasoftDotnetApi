using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IInvitationService
{
    Task<ActionResult> CancelAsync(string id, User user);
    Task<ActionResult> AcceptAsync(string id, User user);
    Task<ActionResult<InvitationDto>> CreateEventInvitationAsync(string senderId, string receiverId, string eventId);
    Task<ActionResult<InvitationDto>> CreateWorkspaceInvitationAsync(string senderId, string receiverId, string workspaceId);
    Task<ActionResult<List<InvitationDto>>> GetByReceiverUserAsync(User user);

}
