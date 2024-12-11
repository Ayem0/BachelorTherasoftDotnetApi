using System;
using BachelorTherasoftDotnetApi.src.Dtos.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IUserService
{
    Task<UserDto?> GetByIdAsync(string id);
}