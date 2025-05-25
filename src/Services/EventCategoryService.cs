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

public class EventCategoryService : IEventCategoryService
{
    private readonly IEventCategoryRepository _eventCategoryRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IMapper _mapper;
    private readonly IRedisService _cache;
    private readonly ISocketService _socket;
    private static readonly TimeSpan ttl = TimeSpan.FromMinutes(10);

    public EventCategoryService(
        IEventCategoryRepository eventCategoryRepository,
        IWorkspaceRepository workspaceRepository,
        IMapper mapper,
        IRedisService cache,
        ISocketService socket
    )
    {
        _eventCategoryRepository = eventCategoryRepository;
        _workspaceRepository = workspaceRepository;
        _workspaceRepository = workspaceRepository;
        _mapper = mapper;
        _cache = cache;
        _socket = socket;
    }

    public async Task<EventCategoryDto> CreateAsync(string workspaceId, CreateEventCategoryRequest req)
    {
        var workspace = await _cache.GetOrSetAsync(
            CacheKeys.Workspace(workspaceId),
            () => _workspaceRepository.GetByIdAsync(workspaceId),
            ttl
        ) ?? throw new NotFoundException("Workspace", workspaceId);

        var eventCategory = new EventCategory(workspace, req.Name, req.Icon, req.Color, req.Description) { Workspace = workspace };

        var created = await _eventCategoryRepository.CreateAsync(eventCategory);
        var dto = _mapper.Map<EventCategoryDto>(eventCategory);

        await _socket.NotififyGroup(workspaceId, "EventCategoryCreated", dto);
        await _cache.SetAsync(CacheKeys.EventCategory(workspaceId, created.Id), created, ttl);
        await _cache.DeleteAsync(CacheKeys.EventCategories(workspaceId));

        return dto;
    }

    public async Task<EventCategoryDto> UpdateAsync(string workspaceId, string id, UpdateEventCategoryRequest req)
    {
        var key = CacheKeys.EventCategory(workspaceId, id);
        var EventCategory = await _cache.GetOrSetAsync(key, () => _eventCategoryRepository.GetByIdAsync(id), ttl)
            ?? throw new NotFoundException("EventCategory", id);

        EventCategory.Name = req.Name ?? EventCategory.Name;
        EventCategory.Description = req.Description ?? EventCategory.Description;

        var updated = await _eventCategoryRepository.UpdateAsync(EventCategory);
        var dto = _mapper.Map<EventCategoryDto>(updated);

        await _cache.SetAsync(key, dto, TimeSpan.FromMinutes(10));
        await _socket.NotififyGroup(workspaceId, "EventCategoryUpdated", dto);
        await _cache.DeleteAsync(CacheKeys.EventCategories(workspaceId));

        return dto;
    }

    public async Task<bool> DeleteAsync(string workspaceId, string id)
    {
        var key = CacheKeys.EventCategory(workspaceId, id);
        var EventCategory = await _cache.GetOrSetAsync(key, () => _eventCategoryRepository.GetByIdAsync(id), ttl)
            ?? throw new NotFoundException("EventCategory", id);

        var success = await _eventCategoryRepository.DeleteAsync(EventCategory);
        if (success)
        {
            await _socket.NotififyGroup(EventCategory.WorkspaceId, "EventCategoryDeleted", id);
            await _cache.DeleteAsync([
                CacheKeys.EventCategories(workspaceId),
                CacheKeys.EventCategory(workspaceId, id)
            ]);
        }
        return success;
    }

    public Task<EventCategoryDto?> GetByIdAsync(string workspaceId, string id)
    => _cache.GetOrSetAsync<EventCategory?, EventCategoryDto?>(
        CacheKeys.EventCategory(workspaceId, id),
        () => _eventCategoryRepository.GetByIdAsync(id),
        ttl
    );

    public Task<List<EventCategoryDto>> GetByWorkspaceIdAsync(string workspaceId)
    => _cache.GetOrSetAsync<List<EventCategory>, List<EventCategoryDto>>(
        CacheKeys.EventCategories(workspaceId),
        () => _eventCategoryRepository.GetByWorkpaceIdAsync(workspaceId),
        ttl
    );
}
