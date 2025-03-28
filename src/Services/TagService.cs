using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Services;

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IMapper _mapper;
    public TagService(ITagRepository tagRepository, IWorkspaceRepository workspaceRepository, IMapper mapper)
    {
        _tagRepository = tagRepository;
        _workspaceRepository = workspaceRepository;
        _mapper = mapper;
    }

    public async Task<TagDto> CreateAsync(CreateTagRequest request)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(request.WorkspaceId) ?? throw new NotFoundException("Workspace", request.WorkspaceId);

        var tag = new Tag(workspace, request.Name, request.Icon, request.Color, request.Description) { Workspace = workspace };

        await _tagRepository.CreateAsync(tag);

        return _mapper.Map<TagDto>(tag);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        return await _tagRepository.DeleteAsync(id);
    }

    public async Task<TagDto> GetByIdAsync(string id)
    {
        var tag = await _tagRepository.GetByIdAsync(id) ?? throw new NotFoundException("Tag", id);

        return _mapper.Map<TagDto>(tag);
    }

    public async Task<TagDto> UpdateAsync(string id, UpdateTagRequest req)
    {
        var tag = await _tagRepository.GetByIdAsync(id) ?? throw new NotFoundException("Tag", id);

        tag.Name = req.Name ?? tag.Name;
        tag.Icon = req.Icon ?? tag.Icon;
        tag.Description = req.Description ?? tag.Description;
        tag.Color = req.Color ?? tag.Color;

        await _tagRepository.UpdateAsync(tag);

        return _mapper.Map<TagDto>(tag);
    }

    public async Task<List<TagDto>> GetByWorkpsaceIdAsync(string id)
    {
        var res = await _tagRepository.GetByWorkpaceIdAsync(id);
        return _mapper.Map<List<TagDto>>(res);
    }
}
