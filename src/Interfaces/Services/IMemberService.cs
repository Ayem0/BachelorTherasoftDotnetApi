using System;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Enums;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IMemberService
{
    Task<Response<MemberDto?>> GetByIdAsync(string id);
    Task<Response<MemberDto?>> CreateAsync(string workspaceId, string userId);
    Task<Response<string>> DeleteAsync(string id);
    Task<Response<MemberDto?>> UpdateAsync(string id, Status? newStatus);
}
