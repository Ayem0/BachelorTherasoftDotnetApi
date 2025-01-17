using System;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IWorkspaceAuthorizationService
{
    Task<object?> GetEntityById(string tableName, string id);
}
