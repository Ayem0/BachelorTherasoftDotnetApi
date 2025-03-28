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
        return await _context.Participant
            .Where(x => x.WorkspaceId == id && x.Workspace.DeletedAt == null && x.DeletedAt == null)
            .ToListAsync();
    }
}
