using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface ITagService
{
    Task<TagDto> GetByIdAsync(string id);
    Task<TagDto> CreateAsync(CreateTagRequest request);
    Task<bool> DeleteAsync(string id);
    Task<TagDto> UpdateAsync(string id, UpdateTagRequest request);
    Task<List<TagDto>> GetByWorkpsaceIdAsync(string id);
}
