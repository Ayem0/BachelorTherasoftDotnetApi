using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Repositories;

public interface IUserRepository
{

    Task<User?> GetByIdAsync(string id);
    Task<User?> GetByIdJoinWorkspaceAsync(string id);
    Task<User> UpdateAsync(User user);
}
