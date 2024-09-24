using System;
using BachelorTherasoftDotnetApi.Base;
using BachelorTherasoftDotnetApi.Interfaces;
using BachelorTherasoftDotnetApi.Models;

namespace BachelorTherasoftDotnetApi.Services;

public class WorkspaceRoleService : IWorkspaceRoleService
{
    private readonly IBaseRepository<WorkspaceRole> _workspaceRoleRepository;

    public WorkspaceRoleService(IBaseRepository<WorkspaceRole> workspaceRoleRepository)
    {
        _workspaceRoleRepository = workspaceRoleRepository;
    }

}
