using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class TagRepository : BaseMySqlRepository<Tag>, ITagRepository
{
    public TagRepository(MySqlDbContext context) : base(context)
    {
    }

    public async new Task<Tag?> GetByIdAsync(string id)
    {
        return await _context.Tag
            .Where(x => x.Id == id && x.DeletedAt == null && x.Workspace.DeletedAt == null)
            .FirstOrDefaultAsync();
    }

    public async Task<Tag?> GetByIdJoinWorkspaceAsync(string id)
    {
        return await _context.Tag
            .Include(x => x.Workspace)        
            .Where(x => x.Id == id && x.DeletedAt == null && x.Workspace.DeletedAt == null)
            .FirstOrDefaultAsync();
    }
}
