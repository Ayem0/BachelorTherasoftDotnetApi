using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.AspNetCore.Identity;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByIdJoinWorkspaceAsync(string id);
    Task<User?> GetJoinContactsAndBlockedUsersByIdAsync(string id);
    Task<User?> GetJoinContactsAndBlockedUsersByEmailAsync(string email);
    Task<List<User>> GetContactsByIdAsync(string id);
    Task<List<User>> GetByWorkspaceIdAsync(string id);
}
