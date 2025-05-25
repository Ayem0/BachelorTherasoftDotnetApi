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

public class AreaService : IAreaService
{
    private readonly IAreaRepository _areaRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IMapper _mapper;
    private readonly IRedisService _cache;
    private readonly ISocketService _socket;
    private static readonly TimeSpan ttl = TimeSpan.FromMinutes(10);

    public AreaService(
        IAreaRepository areaRepository,
        ILocationRepository locationRepository,
        IMapper mapper,
        IRedisService cache,
        ISocketService socket,
        IWorkspaceRepository workspaceRepository
    )
    {
        _areaRepository = areaRepository;
        _locationRepository = locationRepository;
        _mapper = mapper;
        _cache = cache;
        _socket = socket;
        _workspaceRepository = workspaceRepository;
    }

    public async Task<AreaDto> CreateAsync(string workspaceId, CreateAreaRequest req)
    {
        var workspace = await _cache.GetOrSetAsync(
            CacheKeys.Workspace(workspaceId),
            () => _workspaceRepository.GetByIdAsync(workspaceId),
            ttl
        ) ?? throw new NotFoundException("Workspace", workspaceId);

        var location = await _cache.GetOrSetAsync(
            CacheKeys.Location(workspaceId, req.LocationId),
            () => _locationRepository.GetByIdAsync(req.LocationId),
            ttl
        ) ?? throw new NotFoundException("Location", req.LocationId);

        var area = new Area(workspace, location, req.Name, req.Description) { Location = location, Workspace = workspace };

        var created = await _areaRepository.CreateAsync(area);
        var dto = _mapper.Map<AreaDto>(area);

        await _socket.NotififyGroup(workspaceId, "AreaCreated", dto);
        await _cache.SetAsync(CacheKeys.Area(workspaceId, created.Id), created, ttl);
        await _cache.DeleteAsync(CacheKeys.Areas(workspaceId));

        return dto;
    }

    public async Task<AreaDto> UpdateAsync(string workspaceId, string id, UpdateAreaRequest req)
    {
        var key = CacheKeys.Area(workspaceId, id);
        var area = await _cache.GetOrSetAsync(key, () => _areaRepository.GetByIdAsync(id), ttl)
            ?? throw new NotFoundException("Area", id);

        area.Name = req.Name ?? area.Name;
        area.Description = req.Description ?? area.Description;

        var updated = await _areaRepository.UpdateAsync(area);
        var dto = _mapper.Map<AreaDto>(updated);

        await _cache.SetAsync(key, dto, ttl);
        await _socket.NotififyGroup(workspaceId, "AreaUpdated", dto);
        await _cache.DeleteAsync(CacheKeys.Areas(workspaceId));

        return dto;
    }

    public async Task<bool> DeleteAsync(string workspaceId, string id)
    {
        var key = CacheKeys.Area(workspaceId, id);
        var area = await _cache.GetOrSetAsync(key, () => _areaRepository.GetByIdAsync(id), ttl)
            ?? throw new NotFoundException("Area", id);

        var success = await _areaRepository.DeleteAsync(area);
        if (success)
        {
            await _socket.NotififyGroup(area.WorkspaceId, "AreaDeleted", id);
            await _cache.DeleteAsync([
                CacheKeys.LocationAreas(workspaceId, area.LocationId),
                CacheKeys.Areas(workspaceId),
                CacheKeys.Area(workspaceId, id)
            ]);
        }
        return success;
    }

    public Task<AreaDto?> GetByIdAsync(string workspaceId, string id)
    => _cache.GetOrSetAsync<Area?, AreaDto?>(
        CacheKeys.Area(workspaceId, id),
        () => _areaRepository.GetByIdAsync(id),
        ttl
    );

    public Task<List<AreaDto>> GetByWorkspaceIdAsync(string workspaceId)
    => _cache.GetOrSetAsync<List<Area>, List<AreaDto>>(
        CacheKeys.Areas(workspaceId),
        () => _areaRepository.GetAreasByWorkspaceIdAsync(workspaceId),
        ttl
    );

    public Task<List<AreaDto>> GetByLocationIdAsync(string workspaceId, string locationId)
    => _cache.GetOrSetAsync<List<Area>, List<AreaDto>>(
        CacheKeys.LocationAreas(workspaceId, locationId),
        () => _areaRepository.GetAreasByLocationIdAsync(locationId),
        ttl
    );
}
