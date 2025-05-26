using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;

namespace BachelorTherasoftDotnetApi.src.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ISocketService _socket;
    public UserService(
        IMapper mapper,
        IUserRepository userRepository,
        ISocketService hub
    )
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _socket = hub;
    }

    public async Task<UserDto?> GetByIdAsync(string id)
    => _mapper.Map<UserDto?>(await _userRepository.GetByIdAsync(id));

    public async Task<List<UserDto>> GetContactsByIdAsync(string id)
    => _mapper.Map<List<UserDto>>(await _userRepository.GetContactsByIdAsync(id));

    public async Task<List<UserDto>> GetByWorkspaceIdAsync(string id)
    => _mapper.Map<List<UserDto>>(await _userRepository.GetByWorkspaceIdAsync(id));

    public async Task<UserDto> UpdateAsync(string id, UpdateUserRequest req)
    {
        var user = await _userRepository.GetByIdAsync(id) ?? throw new NotFoundException("User", id);

        user.FirstName = req.FirstName ?? user.FirstName;
        user.LastName = req.LastName ?? user.LastName;

        var updated = await _userRepository.UpdateAsync(user);
        var dto = _mapper.Map<UserDto>(updated);
        await _socket.NotififyUser(id, "UserUpdated", dto);
        return dto;
    }
}
