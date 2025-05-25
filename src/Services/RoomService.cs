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

public class RoomService : IRoomService
{
    private readonly IAreaRepository _areaRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IMapper _mapper;
    private readonly IRedisService _cache;
    private readonly ISocketService _socket;
    private static readonly TimeSpan ttl = TimeSpan.FromMinutes(10);

    public RoomService(
        IAreaRepository areaRepository,
        IRoomRepository roomRepository,
        IMapper mapper,
        IRedisService cache,
        ISocketService hub,
        IWorkspaceRepository workspaceRepository
    )
    {
        _areaRepository = areaRepository;
        _roomRepository = roomRepository;
        _mapper = mapper;
        _socket = hub;
        _cache = cache;
        _workspaceRepository = workspaceRepository;
    }

    public async Task<RoomDto> CreateAsync(string workspaceId, CreateRoomRequest request)
    {
        var workspace = await _cache.GetOrSetAsync(
            CacheKeys.Workspace(workspaceId),
            () => _workspaceRepository.GetByIdAsync(workspaceId),
            ttl
        ) ?? throw new NotFoundException("Workspace", workspaceId);

        var area = await _cache.GetOrSetAsync(
            CacheKeys.Area(workspaceId, request.AreaId),
            () => _areaRepository.GetByIdAsync(request.AreaId),
            ttl
        ) ?? throw new NotFoundException("Area", request.AreaId);

        var room = new Room(area.Workspace, area, request.Name, request.Description) { Area = area, Workspace = area.Workspace };

        var created = await _roomRepository.CreateAsync(room);
        var dto = _mapper.Map<RoomDto>(room);

        await _socket.NotififyGroup(workspaceId, "RoomCreated", dto);
        await _cache.SetAsync(CacheKeys.Room(workspaceId, created.Id), created, ttl);
        await _cache.DeleteAsync([CacheKeys.AreaRooms(workspaceId, area.Id), CacheKeys.Rooms(workspaceId)]);

        return dto;
    }

    public async Task<RoomDto> UpdateAsync(string workspaceId, string id, UpdateRoomRequest request)
    {
        var key = CacheKeys.Room(workspaceId, id);
        var room = await _cache.GetOrSetAsync(key, () => _roomRepository.GetByIdAsync(id), ttl)
            ?? throw new NotFoundException("Room", id);

        room.Name = request.NewName ?? room.Name;
        room.Description = request.NewDescription ?? room.Description;

        var updated = await _roomRepository.UpdateAsync(room);
        var dto = _mapper.Map<RoomDto>(updated);

        await _cache.SetAsync(key, dto, TimeSpan.FromMinutes(10));
        await _socket.NotififyGroup(workspaceId, "RoomUpdated", dto);
        await _cache.DeleteAsync([CacheKeys.AreaRooms(workspaceId, room.AreaId), CacheKeys.Rooms(workspaceId)]);

        return dto;
    }

    public async Task<bool> DeleteAsync(string workspaceId, string id)
    {
        var key = CacheKeys.Room(workspaceId, id);
        var room = await _cache.GetOrSetAsync(key, () => _roomRepository.GetByIdAsync(id), ttl)
            ?? throw new NotFoundException("Room", id);

        var success = await _roomRepository.DeleteAsync(room);
        if (success)
        {
            await _socket.NotififyGroup(room.WorkspaceId, "RoomDeleted", id);
            await _cache.DeleteAsync([
                CacheKeys.AreaRooms(workspaceId, room.AreaId),
                CacheKeys.Rooms(workspaceId),
                CacheKeys.Room(workspaceId, id)
            ]);
        }
        return success;
    }

    public Task<RoomDto?> GetByIdAsync(string workspaceId, string id)
    => _cache.GetOrSetAsync<Room?, RoomDto?>(
        CacheKeys.Room(workspaceId, id),
        () => _roomRepository.GetByIdAsync(id),
        ttl
    );

    public Task<List<RoomDto>> GetByWorkspaceIdAsync(string workspaceId)
    => _cache.GetOrSetAsync<List<Room>, List<RoomDto>>(
        CacheKeys.Rooms(workspaceId),
        () => _roomRepository.GetByWorkspaceIdAsync(workspaceId),
        ttl
    );

    public Task<List<RoomDto>> GetByAreaIdAsync(string workspaceId, string areaId)
    => _cache.GetOrSetAsync<List<Room>, List<RoomDto>>(
        CacheKeys.AreaRooms(workspaceId, areaId),
        () => _roomRepository.GetByAreaIdAsync(workspaceId),
        ttl
    );
}
