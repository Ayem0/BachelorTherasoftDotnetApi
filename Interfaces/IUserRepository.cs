namespace BachelorTherasoftDotnetApi.Interfaces;

public interface IUserRepository
{
    Task UpdateFirstNameAsync(string userId, string firstName);
    Task UpdateLastNameAsync(string userId, string lastName);
    Task UpdateEmailAsync(string userId, string email);
    Task UpdatePasswordAsync(string userId, string password);
    Task DeleteAsync(string userId);
}
