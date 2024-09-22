using BachelorTherasoftDotnetApi.Models;
using BachelorTherasoftDotnetApi.Interfaces;
using BachelorTherasoftDotnetApi.Databases;
using Microsoft.EntityFrameworkCore;
using BachelorTherasoftDotnetApi.Base;

namespace BachelorTherasoftDotnetApi.Repositories;

public class WorkspaceRepository : BaseRepository<Workspace>, IWorkspaceRepository
{
    public WorkspaceRepository(MySqlDbContext context) : base(context)
    {
    }
    public async new Task<Workspace?> GetByIdAsync(string id)
{
    return await _context.Workspace
        .Include(w => w.Users)
        .Include(w => w.WorkspaceRoles)
        .Where(w => w.Id == id && w.DeletedAt == null)
        .FirstOrDefaultAsync();
}
}
