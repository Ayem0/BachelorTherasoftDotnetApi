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
// TODO voir si mettre les areas dans le LocationDto
public class LocationService : ILocationService
{
    private readonly ILocationRepository _locationRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IMapper _mapper;
    private readonly IRedisService _cache;
    private readonly ISocketService _socket;
    private static readonly TimeSpan ttl = TimeSpan.FromMinutes(10);
    public LocationService(
        ILocationRepository locationRepository,
        IWorkspaceRepository workspaceRepository,
        IMapper mapper,
        IRedisService cache,
        ISocketService socket
    )
    {
        _locationRepository = locationRepository;
        _workspaceRepository = workspaceRepository;
        _mapper = mapper;
        _cache = cache;
        _socket = socket;
    }

    public async Task<LocationDto> CreateAsync(string workspaceId, CreateLocationRequest req)
    {
        var workspace = await _cache.GetOrSetAsync(
            CacheKeys.Workspace(workspaceId),
            () => _workspaceRepository.GetByIdAsync(workspaceId),
            ttl
        ) ?? throw new NotFoundException("Workspace", workspaceId);

        var location = new Location(workspace, req.Name, req.Description, req.Address, req.City, req.Country) { Workspace = workspace };

        var created = await _locationRepository.CreateAsync(location);
        var dto = _mapper.Map<LocationDto>(location);

        await _socket.NotififyGroup(workspaceId, "LocationCreated", dto);
        await _cache.SetAsync(CacheKeys.Location(workspaceId, created.Id), created, ttl);
        await _cache.DeleteAsync(CacheKeys.Locations(workspaceId));

        return dto;
    }

    public async Task<LocationDto> UpdateAsync(string workspaceId, string id, UpdateLocationRequest req)
    {
        var key = CacheKeys.Location(workspaceId, id);
        var location = await _cache.GetOrSetAsync(key, () => _locationRepository.GetByIdAsync(id), ttl)
            ?? throw new NotFoundException("Location", id);

        location.Name = req.Name ?? location.Name;
        location.Description = req.Description ?? location.Description;

        var updated = await _locationRepository.UpdateAsync(location);
        var dto = _mapper.Map<LocationDto>(updated);

        await _cache.SetAsync(key, dto, TimeSpan.FromMinutes(10));
        await _socket.NotififyGroup(workspaceId, "LocationUpdated", dto);
        await _cache.DeleteAsync(CacheKeys.Locations(workspaceId));

        return dto;
    }

    public async Task<bool> DeleteAsync(string workspaceId, string id)
    {
        var key = CacheKeys.Location(workspaceId, id);
        var location = await _cache.GetOrSetAsync(key, () => _locationRepository.GetByIdAsync(id), ttl)
            ?? throw new NotFoundException("Location", id);

        var success = await _locationRepository.DeleteAsync(location);
        if (success)
        {
            await _socket.NotififyGroup(location.WorkspaceId, "LocationDeleted", id);
            await _cache.DeleteAsync([
                CacheKeys.Locations(workspaceId),
                CacheKeys.Location(workspaceId, id)
            ]);
        }
        return success;
    }

    public Task<LocationDto?> GetByIdAsync(string workspaceId, string id)
    => _cache.GetOrSetAsync<Location?, LocationDto?>(
        CacheKeys.Location(workspaceId, id),
        () => _locationRepository.GetByIdAsync(id),
        ttl
    );

    public Task<List<LocationDto>> GetByWorkspaceIdAsync(string workspaceId)
    => _cache.GetOrSetAsync<List<Location>, List<LocationDto>>(
        CacheKeys.Locations(workspaceId),
        () => _locationRepository.GetByWorkspaceIdAsync(workspaceId),
        ttl
    );

}
