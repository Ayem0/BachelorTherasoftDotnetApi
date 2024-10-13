using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Utils;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Services;

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IMapper _mapper;
    public TagService(ITagRepository tagRepository, IWorkspaceRepository workspaceRepository,  IMapper mapper)
    {
        _tagRepository = tagRepository;
        _workspaceRepository = workspaceRepository;
        _mapper = mapper;
    }

    public async Task<ActionResult<TagDto>> CreateAsync(CreateTagRequest request)
    {
        var res = await _workspaceRepository.GetEntityByIdAsync(request.WorkspaceId);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        if (res.Data == null) return Response.NotFound(request.WorkspaceId, nameof(res.Data));

        var tag = new Tag(res.Data, request.Name, request.Icon, request.Description){ Workspace = res.Data };
        
        var res2 = await _tagRepository.CreateAsync(tag);
        if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);

        return Response.CreatedAt(_mapper.Map<TagDto>(tag));
    }

    public async Task<ActionResult> DeleteAsync(string id)
    {
        var res = await _tagRepository.DeleteAsync(id);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);

        return Response.NoContent();
    }

    public async Task<ActionResult<TagDto>> GetByIdAsync(string id)
    {
        var tag = await _tagRepository.GetByIdAsync<TagDto>(id);
        if (tag == null) return Response.NotFound(id, nameof(tag));

        return Response.Ok(tag);
    }

    public async Task<ActionResult<TagDto>> UpdateAsync(string id, UpdateTagRequest request)
    {
        if (request.NewName == null && request.NewDescription == null && request.NewDescription == null) 
            return Response.BadRequest("At least one field is required.", null);

        var res = await _tagRepository.GetEntityByIdAsync(id);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        if (res.Data == null) return Response.NotFound(id, nameof(res.Data));

        res.Data.Name = request.NewName ?? res.Data.Name;
        res.Data.Icon = request.NewIcon ?? res.Data.Icon;
        res.Data.Description = request.NewDescription ?? res.Data.Description;

        await _tagRepository.UpdateAsync(res.Data);

        return Response.Ok(_mapper.Map<TagDto>(res.Data));
    }
}
