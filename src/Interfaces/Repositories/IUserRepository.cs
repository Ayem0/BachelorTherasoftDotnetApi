using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.AspNetCore.Identity;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByIdJoinWorkspaceAsync(string id);
    Task<User[]> UpdateMultipleAsync(User[] users);
    Task<User?> GetByIdJoinContactsAndBlockedUsersAsync(string id);
    Task<User?> GetByEmailJoinContactsAndBlockedUsersAsync(string email);
    Task<List<User>> GetUserContactsByUserIdAsync(string id);
}
