using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface ITagService
{
    Task<ActionResult<TagDto>> GetByIdAsync(string id);
    Task<ActionResult<TagDto>> CreateAsync(CreateTagRequest request);
    Task<ActionResult> DeleteAsync(string id);
    Task<ActionResult<TagDto>> UpdateAsync(string id, UpdateTagRequest request);
}
