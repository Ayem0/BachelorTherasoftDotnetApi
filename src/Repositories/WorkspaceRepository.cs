using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Base;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class WorkspaceRepository : BaseRepository<Workspace>, IWorkspaceRepository
{
    public WorkspaceRepository(MySqlDbContext context, ILogger<Workspace> logger) : base(context, logger)
    {
    }

    public async Task<List<Workspace>> GetByUserIdAsync(string id)
    {
        try
        {
            return await _dbSet
                .Where(w => w.Users.Any(x => x.Id == id))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error getting workspace for user with Id '{id}' : {errorMessage}", id, ex.Message);
            throw new DbException(DbAction.Read, "Workspace");
        }
    }
}
