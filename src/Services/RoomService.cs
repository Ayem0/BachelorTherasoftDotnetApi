using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Interfaces;
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

    public async Task<RoomDto?> CreateAsync(string name, string areaId, string? description)
    {
        var area = await _areaRepository.GetByIdAsync(areaId);
        if (area == null) return null;

        var room = new Room(area, name, description) { Area = area };
        await _roomRepository.CreateAsync(room);

        return new RoomDto(room);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        if (room == null) return false;

        await _roomRepository.DeleteAsync(room);
        return true;
    }

    public async Task<RoomDto?> GetByIdAsync(string id)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        if (room == null) return null;

        return new RoomDto(room);
    }

    public async Task<RoomDto?> UpdateAsync(string id, string? newName, string? newDescription)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        if (room == null || (newName == null && newDescription == null)) return null;

        room.Name = newName ?? room.Name;
        room.Description = newDescription ?? room.Description;

        await _roomRepository.UpdateAsync(room);
        return new RoomDto(room);
    }
}
