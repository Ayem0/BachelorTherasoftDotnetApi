using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IAreaService
{
    Task<Response<AreaDto?>> GetByIdAsync(string id);
    Task<Response<AreaDto?>> CreateAsync(string locationId, string name, string? description);
    Task<Response<string>> DeleteAsync(string id);
    Task<Response<AreaDto?>> UpdateAsync(string id, string? newName, string? newDescription);
}
