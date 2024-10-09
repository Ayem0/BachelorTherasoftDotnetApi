
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IRoomService
{
    Task<Response<RoomDto?>> GetByIdAsync(string id);
    Task<Response<RoomDto?>> CreateAsync(CreateRoomRequest request);
    Task<Response<string>> DeleteAsync(string id);
    Task<Response<RoomDto?>> UpdateAsync(string id, UpdateRoomRequest request);
}
