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
    private readonly ISocketService _socket;

    public RoomService(
        IAreaRepository areaRepository,
        IRoomRepository roomRepository,
        IMapper mapper,
        ISocketService hub,
        IWorkspaceRepository workspaceRepository
    )
    {
        _areaRepository = areaRepository;
        _roomRepository = roomRepository;
        _mapper = mapper;
        _socket = hub;
        _workspaceRepository = workspaceRepository;
    }

    public async Task<RoomDto> CreateAsync(CreateRoomRequest req)
    {
        var area = await _areaRepository.GetByIdAsync(req.AreaId) ?? throw new NotFoundException("Area", req.AreaId);
        var workspace = await _workspaceRepository.GetByIdAsync(area.WorkspaceId) ?? throw new NotFoundException("Workspace", area.WorkspaceId);
        var room = new Room(workspace, area, req.Name, req.Description) { Area = area, Workspace = workspace };
        var created = await _roomRepository.CreateAsync(room);
        var dto = _mapper.Map<RoomDto>(created);
        await _socket.NotififyGroup(created.WorkspaceId, "RoomCreated", dto);
        return dto;
    }

    public async Task<RoomDto> UpdateAsync(string id, UpdateRoomRequest req)
    {
        var room = await _roomRepository.GetByIdAsync(id) ?? throw new NotFoundException("Room", id);

        room.Name = req.NewName ?? room.Name;
        room.Description = req.NewDescription ?? room.Description;

        var updated = await _roomRepository.UpdateAsync(room);
        var dto = _mapper.Map<RoomDto>(updated);
        await _socket.NotififyGroup(updated.WorkspaceId, "RoomUpdated", dto);
        return dto;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var room = await _roomRepository.GetByIdAsync(id) ?? throw new NotFoundException("Room", id);
        var success = await _roomRepository.DeleteAsync(room);
        if (success)
        {
            await _socket.NotififyGroup(room.WorkspaceId, "RoomDeleted", id);
        }
        return success;
    }

    public async Task<RoomDto?> GetByIdAsync(string id)
    => _mapper.Map<RoomDto?>(await _roomRepository.GetByIdAsync(id));

    public async Task<List<RoomDto>> GetByWorkspaceIdAsync(string workspaceId)
    => _mapper.Map<List<RoomDto>>(await _roomRepository.GetByWorkspaceIdAsync(workspaceId));

    public async Task<List<RoomDto>> GetByAreaIdAsync(string areaId)
    => _mapper.Map<List<RoomDto>>(await _roomRepository.GetByAreaIdAsync(areaId));
}
