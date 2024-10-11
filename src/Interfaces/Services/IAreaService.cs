using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IAreaService
{
    Task<ActionResult<AreaDto>> GetByIdAsync(string id);
    Task<ActionResult<AreaDto>> CreateAsync(string locationId, string name, string? description);
    Task<ActionResult> DeleteAsync(string id);
    Task<ActionResult<AreaDto>> UpdateAsync(string id, string? newName, string? newDescription);
}
