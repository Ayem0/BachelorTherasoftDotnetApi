
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IRoomService
{
    Task<RoomDto?> GetByIdAsync(string workspaceId, string id);
    Task<RoomDto> CreateAsync(string workspaceId, CreateRoomRequest request);
    Task<bool> DeleteAsync(string workspaceId, string id);
    Task<RoomDto> UpdateAsync(string workspaceId, string id, UpdateRoomRequest request);
    Task<List<RoomDto>> GetByAreaIdAsync(string workspaceId, string areaId);
    Task<List<RoomDto>> GetByWorkspaceIdAsync(string id);
}
