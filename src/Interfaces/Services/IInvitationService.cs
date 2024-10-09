using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IInvitationService
{
    Task<IActionResult> CancelAsync(string id, User user);
    Task<IActionResult> AcceptAsync(string id, User user);
    Task<IActionResult> CreateEventInvitationAsync(string senderId, string receiverId, string eventId);
    Task<IActionResult> CreateWorkspaceInvitationAsync(string senderId, string receiverId, string workspaceId);
    Task<IActionResult> GetByReceiverUserAsync(User user);

}
