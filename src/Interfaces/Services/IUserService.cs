using System;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Repositories;

public interface IUserService
{
    Task<UserJoinWorkspaceDto> GetUserJoinWorkspacesByIdAsync(string id);
    Task<UserDto> GetUserInfoAsync(string id);
    Task<UserDto> UpdateAsync(string id, UpdateUserRequest req);
    Task<List<UserDto>> GetUserContactsByIdAsync(string id);
}
