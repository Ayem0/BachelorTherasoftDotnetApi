using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IAreaService
{
    Task<AreaDto?> GetByIdAsync(string workspaceId, string id);
    Task<AreaDto> CreateAsync(string workspaceId, CreateAreaRequest request);
    Task<bool> DeleteAsync(string workspaceId, string id);
    Task<AreaDto> UpdateAsync(string workspaceId, string id, UpdateAreaRequest request);
    Task<List<AreaDto>> GetByLocationIdAsync(string workspaceId, string locationId);
    Task<List<AreaDto>> GetByWorkspaceIdAsync(string workspaceId);
}
