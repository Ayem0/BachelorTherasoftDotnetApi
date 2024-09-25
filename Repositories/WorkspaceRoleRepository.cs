using System;
using BachelorTherasoftDotnetApi.Base;
using BachelorTherasoftDotnetApi.Databases;
using BachelorTherasoftDotnetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.Repositories;

public class WorkspaceRoleRepository : BaseRepository<WorkspaceRole>

{
    public WorkspaceRoleRepository(MySqlDbContext context) : base(context)
    {
    }

    public async new Task<WorkspaceRole?> GetByIdAsync(string id)
    {
        return await _context.WorkspaceRole
            .Include(w => w.Users)
            .Where(w => w.Id == id && w.DeletedAt == null)
            .FirstOrDefaultAsync();
    }
}
