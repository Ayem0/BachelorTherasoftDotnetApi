using System;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Repositories;

public interface IMemberRepository : IBaseRepository<Member>
{
    Task<Member?> GetByUserWorkspaceIds(string userId, string workspaceId);
}
