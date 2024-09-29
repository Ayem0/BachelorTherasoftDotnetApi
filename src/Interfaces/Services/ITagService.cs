using BachelorTherasoftDotnetApi.src.Dtos;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface ITagService
{
    Task<TagDto?> GetByIdAsync(string id);
    Task<TagDto?> CreateAsync(string workspaceId, string name, string icon, string? description);
    Task<bool> DeleteAsync(string id);
    Task<TagDto?> UpdateAsync(string id, string? newName, string? newIcon, string? newDescription);
}
