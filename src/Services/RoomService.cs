using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Services;

public class RoomService : IRoomService
{
    private readonly IAreaRepository _areaRepository;
    private readonly IRoomRepository _roomRepository;
    public RoomService(IAreaRepository areaRepository, IRoomRepository roomRepository)
    {
        _areaRepository = areaRepository;
        _roomRepository = roomRepository;
    }

    public async Task<Response<RoomDto?>> CreateAsync(CreateRoomRequest request)
    {
        var area = await _areaRepository.GetByIdAsync(request.AreaId);
        if (area == null) return new Response<RoomDto?>(success: false, errors: ["Area not found."]);

        var room = new Room(area, request.Name, request.Description) { Area = area };
        await _roomRepository.CreateAsync(room);

        return new Response<RoomDto?>(success: true, content: new RoomDto(room));
    }

    public async Task<Response<string>> DeleteAsync(string id)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        if (room == null) return new Response<string>(success: false, errors: ["Room not found."]);

        await _roomRepository.DeleteAsync(room);
        return new Response<string>(success: true, content: "Room successfully deleted.");
    }

    public async Task<Response<RoomDto?>> GetByIdAsync(string id)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        if (room == null) return new Response<RoomDto?>(success: false, errors: ["Room not found."]);

        return new Response<RoomDto?>(success: true, content: new RoomDto(room));
    }

    public async Task<Response<RoomDto?>> UpdateAsync(string id, UpdateRoomRequest request)
    {
        if (request.NewName == null && request.NewDescription == null) return new Response<RoomDto?>(success: false, errors: ["At least one field is required."]);
        
        var room = await _roomRepository.GetByIdAsync(id);
        if (room == null) return new Response<RoomDto?>(success: false, errors: ["Room not found."]);

        room.Name = request.NewName ?? room.Name;
        room.Description = request.NewDescription ?? room.Description;

        await _roomRepository.UpdateAsync(room);
        return new Response<RoomDto?>(success: true, content: new RoomDto(room));
    }
}
