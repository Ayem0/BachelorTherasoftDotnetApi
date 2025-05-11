using AutoMapper;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class ParticipantRepository : BaseRepository<Participant>, IParticipantRepository
{
    public ParticipantRepository(MySqlDbContext context, ILogger<Participant> logger) : base(context, logger)
    {
    }
    public async Task<List<Participant>> GetByWorkpaceIdAsync(string id)
    {
        return await _dbSet
            .Where(x => x.WorkspaceId == id)
            .ToListAsync();
    }

    public Task<List<Participant>> GetByWorkpaceIdJoinCategoryAsync(string id)
    {
        try
        {
            return _dbSet
                .Include(x => x.ParticipantCategory)
                .Where(x => x.WorkspaceId == id)
                .ToListAsync();
        }
        catch (Exception)
        {
            _logger.LogError("Error while getting participants by workspace id with category join.");
            throw;
        }
    }
}
