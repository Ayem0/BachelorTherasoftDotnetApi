
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Interfaces;
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

    public async Task<TagDto?> CreateAsync(string workspaceId, string name, string icon)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);
        if (workspace == null) return null;

        var tag = new Tag {
            Name = name,
            WorkspaceId = workspace.Id,
            Workspace = workspace,
            Icon = icon
        };

        await _tagRepository.CreateAsync(tag);

        var tagDto = new TagDto {
            Id = tag.Id,
            Name = tag.Name,
            Icon = tag.Icon
        };

        return tagDto;
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

        var TagDto = new TagDto {
            Id = Tag.Id,
            Icon = Tag.Icon,
            Name = Tag.Name
        };

        return TagDto;
    }

    public async Task<bool> UpdateAsync(string id, string? newName, string? newIcon)
    {
        var Tag = await _tagRepository.GetByIdAsync(id);
        if (Tag == null || (newName == null && newIcon == null)) return false;

        Tag.Name = newName ?? Tag.Name;
        Tag.Icon = newIcon ?? Tag.Icon;
        
        await _tagRepository.UpdateAsync(Tag);

        return true;
    }
}
