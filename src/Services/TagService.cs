using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.AspNetCore.Mvc;

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

    public async Task<ActionResult<TagDto>> CreateAsync(CreateTagRequest request)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(request.WorkspaceId);
        if (workspace == null) return new NotFoundObjectResult("Workspace not found.");

        var tag = new Tag(workspace, request.Name, request.Icon, request.Description)
        {
            Workspace = workspace
        };
        
        await _tagRepository.CreateAsync(tag);

        return new CreatedAtActionResult(
            actionName: "Create", 
            controllerName: "Tag", 
            routeValues: new { id = tag.Id }, 
            value: new TagDto(tag)
        );  
    }

    public async Task<ActionResult> DeleteAsync(string id)
    {
        var tag = await _tagRepository.GetByIdAsync(id);
        if (tag == null) return new NotFoundObjectResult("Tag not found.");

        await _tagRepository.DeleteAsync(tag);
        return new OkObjectResult("Tag successfully deleted.");
    }

    public async Task<ActionResult<TagDto>> GetByIdAsync(string id)
    {
        var tag = await _tagRepository.GetByIdAsync(id);
        if (tag == null) return new NotFoundObjectResult("Tag not found.");

        return new OkObjectResult(new TagDto(tag));
    }

    public async Task<ActionResult<TagDto>> UpdateAsync(string id, UpdateTagRequest request)
    {
        if (request.NewName == null && request.NewDescription == null && request.NewDescription == null) 
            return new BadRequestObjectResult("At least one field is required.");

        var tag = await _tagRepository.GetByIdAsync(id);
        if (tag == null) return new NotFoundObjectResult("Tag not found.");

        tag.Name = request.NewName ?? tag.Name;
        tag.Icon = request.NewIcon ?? tag.Icon;
        tag.Description = request.NewDescription ?? tag.Description;

        await _tagRepository.UpdateAsync(tag);
        return new OkObjectResult(new TagDto(tag));
    }
}
