
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
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

    public async Task<Response<TagDto?>> CreateAsync(CreateTagRequest request)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(request.WorkspaceId);
        if (workspace == null) return new Response<TagDto?>(success: false, errors: ["Workspace not found."]);

        var tag = new Tag(workspace, request.Name, request.Icon, request.Description)
        {
            Workspace = workspace
        };
        await _tagRepository.CreateAsync(tag);

        return new Response<TagDto?>(success: true, content: new TagDto(tag));
    }

    public async Task<Response<string>> DeleteAsync(string id)
    {
        var Tag = await _tagRepository.GetByIdAsync(id);
        if (Tag == null) return new Response<string>(success: false, errors: ["Tag not found."]);

        await _tagRepository.DeleteAsync(Tag);
        return new Response<string>(success: true, content: "Tag successfully deleted.");
    }

    public async Task<Response<TagDto?>> GetByIdAsync(string id)
    {
        var tag = await _tagRepository.GetByIdAsync(id);
        if (tag == null) return new Response<TagDto?>(success: false, errors: ["Tag not found."]);

        return new Response<TagDto?>(success: true, content: new TagDto(tag));
    }

    public async Task<Response<TagDto?>> UpdateAsync(string id, UpdateTagRequest request)
    {
        if (request.NewName == null && request.NewDescription == null && request.NewDescription == null) 
            return new Response<TagDto?>(success: false, errors: ["At least one field is required."]);

        var tag = await _tagRepository.GetByIdAsync(id);
        if (tag == null) return new Response<TagDto?>(success: false, errors: ["Tag not found."]);

        tag.Name = request.NewName ?? tag.Name;
        tag.Icon = request.NewIcon ?? tag.Icon;
        tag.Description = request.NewDescription ?? tag.Description;

        await _tagRepository.UpdateAsync(tag);
        return new Response<TagDto?>(success: true, content: new TagDto(tag));
    }
}
