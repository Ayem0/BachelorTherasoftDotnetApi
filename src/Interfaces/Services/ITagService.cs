using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface ITagService
{
    Task<Response<TagDto?>> GetByIdAsync(string id);
    Task<Response<TagDto?>> CreateAsync(CreateTagRequest request);
    Task<Response<string>> DeleteAsync(string id);
    Task<Response<TagDto?>> UpdateAsync(string id, UpdateTagRequest request);
}
