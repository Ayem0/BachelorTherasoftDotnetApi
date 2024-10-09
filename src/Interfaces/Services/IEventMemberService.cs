using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Enums;


namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IEventMemberService
{
    Task<Response<EventMemberDto?>> GetByIdAsync(string id);
    Task<Response<EventMemberDto?>> CreateAsync(string eventId, string memberId);
    Task<Response<string>> DeleteAsync(string id);
    Task<Response<EventMemberDto?>> UpdateAsync(string id, Status? newStatus);
}
