using AutoMapper;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class TagRepository : BaseRepository<Tag>, ITagRepository
{
    public TagRepository(MySqlDbContext context, ILogger<Tag> logger) : base(context, logger)
    {
    }

    public async Task<Tag?> GetByIdJoinWorkspaceAsync(string id)
    {
        try
        {
            return await _dbSet
                .Include(x => x.Workspace)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting Room with Id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Read, "Room", id);
        }
    }

    public async Task<List<Tag>> GetByWorkpaceIdAsync(string id)
    {
        try
        {
            return await _dbSet
                .Where(x => x.WorkspaceId == id)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting Room with Id '{id}' : {ex.Message}");
            throw new DbException(DbAction.Read, "Room", id);
        }
    }
}
