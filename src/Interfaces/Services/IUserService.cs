using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IUserService
{
    public Task<UserDto?> GetByIdAsync(string id);
    public Task<UserDto> UpdateAsync(string id, UpdateUserRequest req);
    public Task<List<UserDto>> GetContactsByIdAsync(string id);
    public Task<List<UserDto>> GetByWorkspaceIdAsync(string id);
}
