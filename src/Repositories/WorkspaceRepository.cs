using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Databases;
using Microsoft.EntityFrameworkCore;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Interfaces;

namespace BachelorTherasoftDotnetApi.src.Repositories;

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
            .Where(w => w.Id == id && w.DeletedAt == null && w.Users.All(u => u.DeletedAt == null) && w.WorkspaceRoles.All(wr => wr.DeletedAt == null))
            .FirstOrDefaultAsync();
    }
}
