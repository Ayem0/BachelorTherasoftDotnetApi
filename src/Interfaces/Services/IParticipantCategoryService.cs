using System;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IParticipantCategoryService
{
    Task<Response<ParticipantCategoryDto?>> GetByIdAsync(string id);
    Task<Response<ParticipantCategoryDto?>> CreateAsync(string workspaceId, string name, string icon);
    Task<Response<string>> DeleteAsync(string id);
    Task<Response<ParticipantCategoryDto?>> UpdateAsync(string id, string? newName, string? newIcon);
}
