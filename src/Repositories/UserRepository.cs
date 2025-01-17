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
            var res = await _context.SaveChangesAsync();
            return res > 0 ? user : throw new DbException(Enums.DbAction.Update, "User", user.Id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating user with Id '{user.Id}' : {ex.Message}");
            throw new DbException(DbAction.Update, "User", user.Id);
        }   
    }
}
