using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface ITagService
{
    Task<TagDto?> GetByIdAsync(string workspaceId, string id);
    Task<TagDto> CreateAsync(string workspaceId, CreateTagRequest request);
    Task<bool> DeleteAsync(string workspaceId, string id);
    Task<TagDto> UpdateAsync(string workspaceId, string id, UpdateTagRequest request);
    Task<List<TagDto>> GetByWorkspaceIdAsync(string id);
}
