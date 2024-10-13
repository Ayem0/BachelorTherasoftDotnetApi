using AutoMapper;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class WorkspaceRoleRepository : BaseMySqlRepository<WorkspaceRole>, IWorkspaceRoleRepository

{
    public WorkspaceRoleRepository(MySqlDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    // public async new Task<WorkspaceRole?> GetByIdAsync(string id)
    // {
    //     return await _context.WorkspaceRole
    //         .Include(wr => wr.Users)
    //         .Where(wr => wr.Id == id && wr.DeletedAt == null && wr.Users.All(u => u.DeletedAt == null))
    //         .FirstOrDefaultAsync();
    // }
}
