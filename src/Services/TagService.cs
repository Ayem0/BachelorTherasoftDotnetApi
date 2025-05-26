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
    private readonly ISocketService _socket;

    public TagService(
        ITagRepository tagRepository,
        IWorkspaceRepository workspaceRepository,
        IMapper mapper,
        ISocketService hub
    )
    {
        _tagRepository = tagRepository;
        _workspaceRepository = workspaceRepository;
        _mapper = mapper;
        _socket = hub;
    }

    public async Task<TagDto> CreateAsync(CreateTagRequest req)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(req.WorkspaceId) ?? throw new NotFoundException("Workspace", req.WorkspaceId);
        var tag = new Tag(workspace, req.Name, req.Icon, req.Color, req.Description) { Workspace = workspace };
        var created = await _tagRepository.CreateAsync(tag);
        var dto = _mapper.Map<TagDto>(created);
        await _socket.NotififyGroup(req.WorkspaceId, "TagCreated", dto);
        return dto;
    }

    public async Task<TagDto> UpdateAsync(string id, UpdateTagRequest req)
    {
        var tag = await _tagRepository.GetByIdAsync(id) ?? throw new NotFoundException("Tag", id);

        tag.Name = req.Name ?? tag.Name;
        tag.Description = req.Description ?? tag.Description;
        tag.Color = req.Color ?? tag.Color;
        tag.Icon = req.Icon ?? tag.Icon;

        var updated = await _tagRepository.UpdateAsync(tag);
        var dto = _mapper.Map<TagDto>(updated);
        await _socket.NotififyGroup(updated.WorkspaceId, "TagUpdated", dto);
        return dto;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var tag = await _tagRepository.GetByIdAsync(id) ?? throw new NotFoundException("Tag", id);
        var success = await _tagRepository.DeleteAsync(tag);
        if (success)
        {
            await _socket.NotififyGroup(tag.WorkspaceId, "TagDeleted", id);
        }
        return success;
    }

    public async Task<TagDto?> GetByIdAsync(string id)
    => _mapper.Map<TagDto?>(await _tagRepository.GetByIdAsync(id));

    public async Task<List<TagDto>> GetByWorkspaceIdAsync(string workspaceId)
    => _mapper.Map<List<TagDto>>(await _tagRepository.GetByWorkpaceIdAsync(workspaceId));
}
