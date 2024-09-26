using BachelorTherasoftDotnetApi.src.Dtos;

namespace BachelorTherasoftDotnetApi.src.Interfaces;

public interface ITagService
{
    Task<TagDto?> GetByIdAsync(string id);
    Task<TagDto?> CreateAsync(string workspaceId, string name, string icon);
    Task<bool> DeleteAsync(string id);
    Task<bool> UpdateAsync(string id, string? newName, string? newIcon);
}
