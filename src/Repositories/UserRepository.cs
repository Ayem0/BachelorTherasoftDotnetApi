using System;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class UserRepository : IUserRepository
{
    private readonly MySqlDbContext _context;
    public UserRepository(MySqlDbContext context)
    {
        _context = context;
    }
    public async Task<User?> GetByIdJoinWorkspaceAsync(string id)
    {
        try
        {
            return await _context.Users
                .Include(u => u.Workspaces)
                .Where(x => x.Id == id && x.DeletedAt == null && x.Workspaces.All(w => w.DeletedAt == null))
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting user with Id '{id}' : {ex.Message}");
            return null;
        }
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        try
        {
            return await _context.Users
                .Where(x => x.Id == id && x.DeletedAt == null)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting user with Id '{id}' : {ex.Message}");
            return null;
        }
    }

    public async Task<User> UpdateAsync(User user)
    {
        try
        {
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return user;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating user with Id '{user.Id}' : {ex.Message}");
            throw new DbException(DbAction.Update, "User", user.Id);
        }
    }

    public async Task<User[]> UpdateMultipleAsync(User[] users)
    {
        try
        {
            foreach (User user in users)
            {
                user.UpdatedAt = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
            return users;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating users with Id '{string.Join(", ", users.Select(x => x.Id))}' : {ex.Message}");
            throw new DbException(DbAction.Update, "Users", string.Join(", ", users.Select(x => x.Id)));
        }
    }

    public async Task<User?> GetByIdJoinContactsAndBlockedUsersAsync(string id)
    {
        try
        {
            return await _context.Users
                .Where(u => u.Id == id && u.DeletedAt == null)
                .Include(u => u.Contacts.Where(c => c.DeletedAt == null))
                .Include(u => u.BlockedUsers.Where(b => b.DeletedAt == null))
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating user with Id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Read, "User", id);
        }
    }

    public async Task<User?> GetByEmailJoinContactsAndBlockedUsersAsync(string email)
    {
        try
        {
            return await _context.Users
                .Where(u => u.Email == email && u.DeletedAt == null)
                .Include(u => u.BlockedUsers.Where(b => b.DeletedAt == null))
                .Include(u => u.Contacts.Where(c => c.DeletedAt == null))
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating user with email '{email}' : {ex.Message}");
            throw new DbException(DbAction.Read, "User", email);
        }
    }

    public async Task<List<User>> GetUserContactsByUserIdAsync(string id)
    {
        try
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .Include(u => u.Contacts)
                .SelectMany(u => u.Contacts.Where(c => c.DeletedAt == null))
                .ToListAsync();

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting contacts from user with id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Read, "User", id);
        }
    }
}
