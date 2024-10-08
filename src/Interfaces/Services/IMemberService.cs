using System;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Enums;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IMemberService
{
    Task<MemberDto?> GetByIdAsync(string id);
    Task<MemberDto?> CreateAsync(string workspaceId, string userId);
    Task<bool> DeleteAsync(string id);
    Task<MemberDto?> UpdateAsync(string id, Status? newStatus);
}
