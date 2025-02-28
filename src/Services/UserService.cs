using System;
using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Hubs;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.AspNetCore.SignalR;

namespace BachelorTherasoftDotnetApi.src.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IInvitationRepository _invitationRepository;
    private readonly IHubContext<WorkspaceHub> _hubContext;
    private readonly IMapper _mapper;
    public UserService(IMapper mapper, IUserRepository userRepository, IInvitationRepository invitationRepository, IHubContext<WorkspaceHub> hubContext)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _invitationRepository = invitationRepository;
        _hubContext = hubContext;
    }

    public async Task<UserDto> GetUserInfoAsync(string id)
    {
        var user = await _userRepository.GetByIdAsync(id) ?? throw new NotFoundException("User", id);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> UpdateAsync(string id, UpdateUserRequest req)
    {
        var user = await _userRepository.GetByIdAsync(id) ?? throw new NotFoundException("User", id);

        user.FirstName = req.FirstName ?? user.FirstName;
        user.LastName = req.LastName ?? user.LastName;

        var res = await _userRepository.UpdateAsync(user);

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
        };
    }

    public async Task<UserDto> GetUserJoinWorkspacesByIdAsync(string id)
    {
        var user = await _userRepository.GetByIdJoinWorkspaceAsync(id) ?? throw new NotFoundException("User", id);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<List<UserDto>> GetUserContactsByIdAsync(string id)
    {
        var contacts = await _userRepository.GetUserContactsByUserIdAsync(id);
        return _mapper.Map<List<UserDto>>(contacts);
    }
}
