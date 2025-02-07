
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IRoomService
{
    Task<RoomDto> GetByIdAsync(string id);
    Task<RoomDto> CreateAsync(CreateRoomRequest request);
    Task<bool> DeleteAsync(string id);
    Task<RoomDto> UpdateAsync(string id, UpdateRoomRequest request);
    Task<List<RoomDto>> GetByAreaIdAsync(string id);
    Task<List<RoomDto>> GetByWorkspaceIdAsync(string id);
}
