using System;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Interfaces;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Repositories;

namespace BachelorTherasoftDotnetApi.src.Services;

public class RoomService : IRoomService
{
    private readonly AreaRepository _areaRepository;
    private readonly RoomRepository _roomRepository;
    public RoomService(AreaRepository areaRepository, RoomRepository roomRepository)
    {
        _areaRepository = areaRepository;
        _roomRepository = roomRepository;
    }

    public async Task<RoomDto?> CreateAsync(string name, string areaId)
    {
        var area = await _areaRepository.GetByIdAsync(areaId);
        if (area == null) return null;

        var room = new Room {
            Name = name,
            AreaId = area.Id,
            Area = area
        };

        await _roomRepository.CreateAsync(room);

        var roomDto = new RoomDto {
            Id = room.Id,
            Name = room.Name,
        };

        return roomDto;
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

        var roomDto = new RoomDto {
            Id = room.Id,
            Name = room.Name
        };

        return roomDto;
    }

    public async Task<bool> UpdateAsync(string id, string newName)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        if (room == null) return false;

        room.Name = newName;
        await _roomRepository.UpdateAsync(room);
        return true;
    }
}
