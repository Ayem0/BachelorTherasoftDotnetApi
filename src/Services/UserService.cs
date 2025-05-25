using System;
using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Hubs;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Utils;
using Microsoft.AspNetCore.SignalR;

namespace BachelorTherasoftDotnetApi.src.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ISocketService _socket;
    private readonly IRedisService _cache;
    private static readonly TimeSpan ttl = TimeSpan.FromMinutes(10);
    public UserService(
        IMapper mapper,
        IUserRepository userRepository,
        ISocketService hub,
        IRedisService cache
    )
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _socket = hub;
        _cache = cache;
    }

    public Task<UserDto?> GetByIdAsync(string id)
    => _cache.GetOrSetAsync<User?, UserDto?>(CacheKeys.User(id), () => _userRepository.GetByIdAsync(id), ttl);

    public Task<List<UserDto>> GetContactsByIdAsync(string id)
    => _cache.GetOrSetAsync<List<User>, List<UserDto>>(CacheKeys.UserContacts(id), () => _userRepository.GetContactsByIdAsync(id), ttl);

    public Task<List<UserDto>> GetByWorkspaceIdAsync(string id)
    => _cache.GetOrSetAsync<List<User>, List<UserDto>>(CacheKeys.WorkspaceUsers(id), () => _userRepository.GetByWorkspaceIdAsync(id), ttl);

    public async Task<UserDto> UpdateAsync(string id, UpdateUserRequest req)
    {
        var key = CacheKeys.User(id);
        var user = await _cache.GetOrSetAsync(key, () => _userRepository.GetByIdAsync(id), ttl)
            ?? throw new NotFoundException("User", id);

        user.FirstName = req.FirstName ?? user.FirstName;
        user.LastName = req.LastName ?? user.LastName;

        var updated = await _userRepository.UpdateAsync(user);
        var dto = _mapper.Map<UserDto>(updated);

        await _cache.SetAsync(key, updated, ttl);
        await _socket.NotififyUser(id, "UserUpdated", dto);

        return dto;
    }

}
