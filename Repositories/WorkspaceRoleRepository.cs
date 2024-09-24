using System;
using BachelorTherasoftDotnetApi.Base;
using BachelorTherasoftDotnetApi.Databases;
using BachelorTherasoftDotnetApi.Interfaces;
using BachelorTherasoftDotnetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.Repositories;

public class WorkspaceRoleRepository : BaseRepository<WorkspaceRole>

{
    public WorkspaceRoleRepository(MySqlDbContext context) : base(context)
    {
    }
}
