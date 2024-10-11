using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.AspNetCore.Mvc;

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

    public async Task<ActionResult<RoomDto>> CreateAsync(CreateRoomRequest request)
    {
        var area = await _areaRepository.GetByIdAsync(request.AreaId);
        if (area == null) return new NotFoundObjectResult("Area not found.");

        var room = new Room(area, request.Name, request.Description) { Area = area };
        await _roomRepository.CreateAsync(room);

        return new CreatedAtActionResult(
            actionName: "Create", 
            controllerName: "Room", 
            routeValues: new { id = room.Id }, 
            value: new RoomDto(room)
        );  
    }

    public async Task<ActionResult> DeleteAsync(string id)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        if (room == null) return new NotFoundObjectResult("Room not found.");

        await _roomRepository.DeleteAsync(room);
        return new OkObjectResult("Successfully deleted room.");
    }

    public async Task<ActionResult<RoomDto>> GetByIdAsync(string id)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        if (room == null) return new NotFoundObjectResult("Room not found.");

        return new OkObjectResult(new RoomDto(room));
    }

    public async Task<ActionResult<RoomDto>> UpdateAsync(string id, UpdateRoomRequest request)
    {
        if (request.NewName == null && request.NewDescription == null) return new BadRequestObjectResult("At least one field is required.");

        var room = await _roomRepository.GetByIdAsync(id);
        if (room == null) return  new NotFoundObjectResult("Room not found.");

        room.Name = request.NewName ?? room.Name;
        room.Description = request.NewDescription ?? room.Description;

        await _roomRepository.UpdateAsync(room);
        return new OkObjectResult(new RoomDto(room));
    }
}
