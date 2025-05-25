using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Hubs;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Utils;

namespace BachelorTherasoftDotnetApi.src.Services;

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IMapper _mapper;
    private readonly ISocketService _socket;
    private readonly IRedisService _cache;
    private static readonly TimeSpan ttl = TimeSpan.FromMinutes(10);

    public TagService(
        ITagRepository tagRepository,
        IWorkspaceRepository workspaceRepository,
        IMapper mapper,
        IRedisService cache,
        ISocketService hub
    )
    {
        _tagRepository = tagRepository;
        _workspaceRepository = workspaceRepository;
        _mapper = mapper;
        _socket = hub;
        _cache = cache;
    }

    public async Task<TagDto> CreateAsync(string workspaceId, CreateTagRequest req)
    {
        var workspace = await _cache.GetOrSetAsync(
            CacheKeys.Workspace(workspaceId),
            () => _workspaceRepository.GetByIdAsync(workspaceId),
            ttl
        ) ?? throw new NotFoundException("Workspace", workspaceId);

        var Tag = new Tag(workspace, req.Name, req.Icon, req.Color, req.Description) { Workspace = workspace };

        var created = await _tagRepository.CreateAsync(Tag);
        var dto = _mapper.Map<TagDto>(Tag);

        await _socket.NotififyGroup(workspaceId, "TagCreated", dto);
        await _cache.SetAsync(CacheKeys.Tag(workspaceId, created.Id), created, ttl);
        await _cache.DeleteAsync(CacheKeys.Tags(workspaceId));

        return dto;
    }

    public async Task<TagDto> UpdateAsync(string workspaceId, string id, UpdateTagRequest req)
    {
        var key = CacheKeys.Tag(workspaceId, id);
        var tag = await _cache.GetOrSetAsync(key, () => _tagRepository.GetByIdAsync(id), ttl)
            ?? throw new NotFoundException("Tag", id);

        tag.Name = req.Name ?? tag.Name;
        tag.Description = req.Description ?? tag.Description;
        tag.Color = req.Color ?? tag.Color;
        tag.Icon = req.Icon ?? tag.Icon;

        var updated = await _tagRepository.UpdateAsync(tag);
        var dto = _mapper.Map<TagDto>(updated);

        await _cache.SetAsync(key, dto, TimeSpan.FromMinutes(10));
        await _socket.NotififyGroup(workspaceId, "TagUpdated", dto);
        await _cache.DeleteAsync(CacheKeys.Tags(workspaceId));

        return dto;
    }

    public async Task<bool> DeleteAsync(string workspaceId, string id)
    {
        var key = CacheKeys.Tag(workspaceId, id);
        var Tag = await _cache.GetOrSetAsync(key, () => _tagRepository.GetByIdAsync(id), ttl)
            ?? throw new NotFoundException("Tag", id);

        var success = await _tagRepository.DeleteAsync(Tag);
        if (success)
        {
            await _socket.NotififyGroup(Tag.WorkspaceId, "TagDeleted", id);
            await _cache.DeleteAsync([
                CacheKeys.Tags(workspaceId),
                CacheKeys.Tag(workspaceId, id)
            ]);
        }
        return success;
    }

    public Task<TagDto?> GetByIdAsync(string workspaceId, string id)
    => _cache.GetOrSetAsync<Tag?, TagDto?>(
        CacheKeys.Tag(workspaceId, id),
        () => _tagRepository.GetByIdAsync(id),
        ttl
    );

    public Task<List<TagDto>> GetByWorkspaceIdAsync(string workspaceId)
    => _cache.GetOrSetAsync<List<Tag>, List<TagDto>>(
        CacheKeys.Tags(workspaceId),
        () => _tagRepository.GetByWorkpaceIdAsync(workspaceId),
        ttl
    );
}
