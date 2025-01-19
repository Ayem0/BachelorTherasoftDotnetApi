using System;
using BachelorTherasoftDotnetApi.src.Base;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IWorkspaceAuthorizationService
{
    Task<BaseAuthorizationModel?> GetEntityById(string tableName, string id);
}
