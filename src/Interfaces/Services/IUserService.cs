using System;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Repositories;

public interface IUserService
{
    Task<UserDto> GetUserJoinWorkspacesByIdAsync(string id);
    Task<UserDto> GetUserInfoAsync(string id);
    Task<UserDto> UpdateAsync(string id, UpdateUserRequest req);
}
