using System;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
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
    public async Task<User?> GetByIdAsync(string id)
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
}
