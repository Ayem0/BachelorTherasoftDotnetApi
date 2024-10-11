
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IRoomService
{
    Task<ActionResult<RoomDto>> GetByIdAsync(string id);
    Task<ActionResult<RoomDto>> CreateAsync(CreateRoomRequest request);
    Task<ActionResult> DeleteAsync(string id);
    Task<ActionResult<RoomDto>> UpdateAsync(string id, UpdateRoomRequest request);
}
