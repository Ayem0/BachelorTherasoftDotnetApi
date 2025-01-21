using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Exceptions;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Services;

public class RoomService : IRoomService
{
    private readonly IAreaRepository _areaRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;
    public RoomService(IAreaRepository areaRepository, IRoomRepository roomRepository, IMapper mapper)
    {
        _areaRepository = areaRepository;
        _roomRepository = roomRepository;
        _mapper = mapper;
    }

    public async Task<RoomDto> CreateAsync(CreateRoomRequest request)
    {
        var area = await _areaRepository.GetByIdJoinWorkspaceAsync(request.AreaId) ?? throw new NotFoundException("Area", request.AreaId);

        var room = new Room(area.Workspace, area, request.Name, request.Description) { Area = area, Workspace = area.Workspace };

        await _roomRepository.CreateAsync(room);

        return _mapper.Map<RoomDto>(room);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        return await _roomRepository.DeleteAsync(id);
    }

    public async Task<RoomDto> GetByIdAsync(string id)
    {
        var room = await _roomRepository.GetByIdAsync(id) ?? throw new NotFoundException("Room", id);

        return _mapper.Map<RoomDto>(room);
    }

    public async Task<List<RoomDto>> GetByAreaIdAsync(string id) {
        var rooms = await _roomRepository.GetByAreaIdAsync(id);
        return _mapper.Map<List<RoomDto>>(rooms);
    }

    public async Task<RoomDto> UpdateAsync(string id, UpdateRoomRequest request)
    {
        var room = await _roomRepository.GetByIdAsync(id) ?? throw new NotFoundException("Room", id);

        room.Name = request.NewName ?? room.Name;
        room.Description = request.NewDescription ?? room.Description;

        await _roomRepository.UpdateAsync(room);

        return _mapper.Map<RoomDto>(room);
    }
}
