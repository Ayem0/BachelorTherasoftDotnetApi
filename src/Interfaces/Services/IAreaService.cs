using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IAreaService
{
    Task<AreaDto?> GetByIdAsync(string id);
    Task<AreaDto> CreateAsync(CreateAreaRequest request);
    Task<bool> DeleteAsync(string id);
    Task<AreaDto> UpdateAsync(string id, UpdateAreaRequest request);
    Task<List<AreaDto>> GetByLocationIdAsync(string locationId);
    Task<List<AreaDto>> GetByWorkspaceIdAsync(string workspaceId);
}
