using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Utils;

namespace BachelorTherasoftDotnetApi.src.Services;

public class ParticipantCategoryService : IParticipantCategoryService
{
    private readonly IParticipantCategoryRepository _participantCategoryRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IMapper _mapper;
    private readonly IRedisService _cache;
    private readonly ISocketService _socket;
    private static readonly TimeSpan ttl = TimeSpan.FromMinutes(10);
    public ParticipantCategoryService(
        IParticipantCategoryRepository participantCategoryRepository,
        IWorkspaceRepository workspaceRepository,
        IMapper mapper,
        IRedisService cache,
        ISocketService socket
        )
    {
        _participantCategoryRepository = participantCategoryRepository;
        _workspaceRepository = workspaceRepository;
        _mapper = mapper;
        _cache = cache;
        _socket = socket;
    }


    public async Task<ParticipantCategoryDto> CreateAsync(string workspaceId, CreateParticipantCategoryRequest req)
    {
        var workspace = await _cache.GetOrSetAsync(
            CacheKeys.Workspace(workspaceId),
            () => _workspaceRepository.GetByIdAsync(workspaceId),
            ttl
        ) ?? throw new NotFoundException("Workspace", workspaceId);

        var ParticipantCategory = new ParticipantCategory(workspace, req.Name, req.Description, req.Color, req.Icon) { Workspace = workspace };

        var created = await _participantCategoryRepository.CreateAsync(ParticipantCategory);
        var dto = _mapper.Map<ParticipantCategoryDto>(ParticipantCategory);

        await _socket.NotififyGroup(workspaceId, "ParticipantCategoryCreated", dto);
        await _cache.SetAsync(CacheKeys.ParticipantCategory(workspaceId, created.Id), created, ttl);
        await _cache.DeleteAsync(CacheKeys.ParticipantCategories(workspaceId));

        return dto;
    }

    public async Task<ParticipantCategoryDto> UpdateAsync(string workspaceId, string id, UpdateParticipantCategoryRequest req)
    {
        var key = CacheKeys.ParticipantCategory(workspaceId, id);
        var ParticipantCategory = await _cache.GetOrSetAsync(key, () => _participantCategoryRepository.GetByIdAsync(id), ttl)
            ?? throw new NotFoundException("ParticipantCategory", id);

        ParticipantCategory.Name = req.Name ?? ParticipantCategory.Name;
        ParticipantCategory.Description = req.Description ?? ParticipantCategory.Description;

        var updated = await _participantCategoryRepository.UpdateAsync(ParticipantCategory);
        var dto = _mapper.Map<ParticipantCategoryDto>(updated);

        await _cache.SetAsync(key, dto, TimeSpan.FromMinutes(10));
        await _socket.NotififyGroup(workspaceId, "ParticipantCategoryUpdated", dto);
        await _cache.DeleteAsync(CacheKeys.ParticipantCategories(workspaceId));

        return dto;
    }

    public async Task<bool> DeleteAsync(string workspaceId, string id)
    {
        var key = CacheKeys.ParticipantCategory(workspaceId, id);
        var ParticipantCategory = await _cache.GetOrSetAsync(key, () => _participantCategoryRepository.GetByIdAsync(id), ttl)
            ?? throw new NotFoundException("ParticipantCategory", id);

        var success = await _participantCategoryRepository.DeleteAsync(ParticipantCategory);
        if (success)
        {
            await _socket.NotififyGroup(ParticipantCategory.WorkspaceId, "ParticipantCategoryDeleted", id);
            await _cache.DeleteAsync([
                CacheKeys.ParticipantCategories(workspaceId),
                CacheKeys.ParticipantCategory(workspaceId, id)
            ]);
        }
        return success;
    }

    public Task<ParticipantCategoryDto?> GetByIdAsync(string workspaceId, string id)
    => _cache.GetOrSetAsync<ParticipantCategory?, ParticipantCategoryDto?>(
        CacheKeys.ParticipantCategory(workspaceId, id),
        () => _participantCategoryRepository.GetByIdAsync(id),
        ttl
    );

    public Task<List<ParticipantCategoryDto>> GetByWorkspaceIdAsync(string workspaceId)
    => _cache.GetOrSetAsync<List<ParticipantCategory>, List<ParticipantCategoryDto>>(
        CacheKeys.ParticipantCategories(workspaceId),
        () => _participantCategoryRepository.GetByWorkpaceIdAsync(workspaceId),
        ttl
    );
}
