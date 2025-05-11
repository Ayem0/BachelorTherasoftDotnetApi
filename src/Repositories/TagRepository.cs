using AutoMapper;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
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
        return await _dbSet
            .Include(x => x.Workspace)
            .Where(x => x.Id == id && x.DeletedAt == null && x.Workspace.DeletedAt == null)
            .FirstOrDefaultAsync();
    }
    public async Task<List<Tag>> GetByWorkpaceIdAsync(string id)
    {
        return await _dbSet
            .Where(x => x.WorkspaceId == id && x.DeletedAt == null && x.Workspace.DeletedAt == null)
            .ToListAsync();
    }
}
