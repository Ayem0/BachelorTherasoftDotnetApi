using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Repositories;

public interface IUserRepository
{

    Task<User?> GetByIdAsync(string id);
    Task<User?> GetByIdJoinWorkspaceAsync(string id);
    Task<User> UpdateAsync(User user);
    Task<User[]> UpdateMultipleAsync(User[] users);
    Task<User?> GetByIdJoinContactsAndBlockedUsersAsync(string id);
    Task<User?> GetByEmailJoinContactsAndBlockedUsersAsync(string email);
    Task<List<User>> GetUserContactsByUserIdAsync(string id);
}
