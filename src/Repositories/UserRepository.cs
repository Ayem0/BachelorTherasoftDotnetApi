using System;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(MySqlDbContext context, ILogger<User> logger) : base(context, logger)
    {
    }
    public async Task<User?> GetByIdJoinWorkspaceAsync(string id)
    {
        try
        {
            return await _dbSet
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

    public async Task<User?> GetJoinContactsAndBlockedUsersByIdAsync(string id)
    {
        try
        {
            return await _dbSet
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

    public async Task<User?> GetJoinContactsAndBlockedUsersByEmailAsync(string email)
    {
        try
        {
            return await _dbSet
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

    public async Task<List<User>> GetContactsByIdAsync(string id)
    {
        try
        {
            return await _dbSet
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

    public async Task<List<User>> GetByWorkspaceIdAsync(string id)
    {
        try
        {
            return await _context.Workspace.Where(w => w.Id == id)
                .SelectMany(u => u.Users)
                .ToListAsync();

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting users from workspace with id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Read, "User", id);
        }
    }
}
