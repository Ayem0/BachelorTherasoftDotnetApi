using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Interfaces.Services;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Utils;
using Microsoft.AspNetCore.Mvc;

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

    public async Task<ActionResult<RoomDto>> CreateAsync(CreateRoomRequest request)
    {
        var res = await _areaRepository.GetEntityByIdAsync(request.AreaId);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        if (res.Data == null) return Response.NotFound(request.AreaId, nameof(res.Data));

        var room = new Room(res.Data, request.Name, request.Description) { Area = res.Data };

        var res2 = await _roomRepository.CreateAsync(room);
        if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);

        return Response.CreatedAt(_mapper.Map<RoomDto>(room));
    }

    public async Task<ActionResult> DeleteAsync(string id)
    {
        var res = await _roomRepository.DeleteAsync(id);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);

        return Response.NoContent();
    }

    public async Task<ActionResult<RoomDto>> GetByIdAsync(string id)
    {
        var room = await _roomRepository.GetByIdAsync<RoomDto>(id);
        if (room == null) return Response.NotFound(id, nameof(room));

        return Response.Ok(room);
    }

    public async Task<ActionResult<RoomDto>> UpdateAsync(string id, UpdateRoomRequest request)
    {
        if (request.NewName == null && request.NewDescription == null) return Response.BadRequest("At least one field is required.", null);

        var res = await _roomRepository.GetEntityByIdAsync(id);
        if (!res.Success) return Response.BadRequest(res.Message, res.Details);
        if (res.Data == null) return Response.NotFound(id, nameof(res.Data));

        res.Data.Name = request.NewName ?? res.Data.Name;
        res.Data.Description = request.NewDescription ?? res.Data.Description;

        var res2 = await _roomRepository.UpdateAsync(res.Data);
        if (!res2.Success) return Response.BadRequest(res2.Message, res2.Details);
        
        return Response.Ok(_mapper.Map<RoomDto>(res.Data));
    }
}
