

using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IEventMemberService
{
    Task<EventMemberDto?> GetByIdAsync(string id);
    Task<EventMemberDto?> CreateAsync(string eventId, string memberId);
    Task<bool> DeleteAsync(string id);
    Task<EventMemberDto?> UpdateAsync(string id, Status? newStatus);
}
