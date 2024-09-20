using System;
using BachelorTherasoftDotnetApi.Models;
using BachelorTherasoftDotnetApi.Interfaces;
using BachelorTherasoftDotnetApi.Databases;

namespace BachelorTherasoftDotnetApi.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly MySqlDbContext _context;
    private readonly AreaRepository _areaRepository; 

    public RoomRepository(MySqlDbContext context, AreaRepository areaRepository)
    {
        _areaRepository = areaRepository;
        _context = context;
    } 

    public async Task CreateRoomAsync(string areaId, string name)
    {
        var area = await _areaRepository.GetAreaAsync(areaId);

        if (area == null) return;

        var room = new Room{
            Area = area,
            AreaId = area.Id,
            Name = name,
        };

        await _context.Room.AddAsync(room);
    }

    public async Task DeleteRoomAsync(string roomId)
    {
        var room = await GetRoomAsync(roomId);

        if (room == null) return;

        
    }

    public Task<Room?> GetRoomAsync(string roomId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Room>?> GetRoomsAsync(string[] roomIds)
    {
        throw new NotImplementedException();
    }

    public Task UpdateRoomAsync(string roomId, string? name, string? areaId)
    {
        throw new NotImplementedException();
    }
}
