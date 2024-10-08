using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Enums;


namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IEventMemberService
{
    Task<EventMemberDto?> GetByIdAsync(string id);
    Task<EventMemberDto?> CreateAsync(string eventId, string memberId);
    Task<bool> DeleteAsync(string id);
    Task<EventMemberDto?> UpdateAsync(string id, Status? newStatus);
}
