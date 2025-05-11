using AutoMapper;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class ParticipantCategoryRepository : BaseRepository<ParticipantCategory>, IParticipantCategoryRepository
{
    public ParticipantCategoryRepository(MySqlDbContext context, ILogger<ParticipantCategory> logger) : base(context, logger)
    {
    }

    public async Task<List<ParticipantCategory>> GetByWorkpaceIdAsync(string id)
    {
        return await _dbSet
            .Where(x => x.WorkspaceId == id && x.Workspace.DeletedAt == null && x.DeletedAt == null)
            .ToListAsync();
    }
}
