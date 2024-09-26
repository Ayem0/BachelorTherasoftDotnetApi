using System;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Interfaces;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class WorkspaceRoleRepository : BaseRepository<WorkspaceRole>, IWorkspaceRoleRepository

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
