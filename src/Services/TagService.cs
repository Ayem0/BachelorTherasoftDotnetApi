
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Services;

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    public TagService(ITagRepository tagRepository, IWorkspaceRepository workspaceRepository)
    {
        _tagRepository = tagRepository;
        _workspaceRepository = workspaceRepository;
    }

    public async Task<TagDto?> CreateAsync(string workspaceId, string name, string icon, string? description)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);
        if (workspace == null) return null;

        var tag = new Tag(workspace, name, icon, description)
        {
            Workspace = workspace
        };
        await _tagRepository.CreateAsync(tag);

        return new TagDto(tag);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var Tag = await _tagRepository.GetByIdAsync(id);
        if (Tag == null) return false;

        await _tagRepository.DeleteAsync(Tag);
        return true;
    }

    public async Task<TagDto?> GetByIdAsync(string id)
    {
        var Tag = await _tagRepository.GetByIdAsync(id);
        if (Tag == null) return null;

        return new TagDto(Tag);
    }

    public async Task<TagDto?> UpdateAsync(string id, string? newName, string? newIcon, string? newDescription)
    {
        var Tag = await _tagRepository.GetByIdAsync(id);
        if (Tag == null || (newName == null && newIcon == null)) return null;

        Tag.Name = newName ?? Tag.Name;
        Tag.Icon = newIcon ?? Tag.Icon;
        Tag.Description = newDescription ?? Tag.Description;

        await _tagRepository.UpdateAsync(Tag);
        return new TagDto(Tag);
    }
}
