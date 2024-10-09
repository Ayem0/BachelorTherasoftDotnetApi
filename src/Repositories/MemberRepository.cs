using System;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class MemberRepository : BaseMySqlRepository<Member>, IMemberRepository
{
    public MemberRepository(MySqlDbContext context) : base(context)
    {
        
    }

    public async Task<Member?> GetByUserWorkspaceIds(string userId, string workspaceId)
    {
        return await _context.Member.Where(x => x.UserId == userId && x.WorkspaceId == workspaceId && x.DeletedAt == null).FirstOrDefaultAsync();
    }
}
