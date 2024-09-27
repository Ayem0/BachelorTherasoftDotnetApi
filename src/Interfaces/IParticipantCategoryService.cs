using System;
using BachelorTherasoftDotnetApi.src.Dtos;

namespace BachelorTherasoftDotnetApi.src.Interfaces;

public interface IParticipantCategoryService
{
    Task<ParticipantCategoryDto?> GetByIdAsync(string id);
    Task<ParticipantCategoryDto?> CreateAsync(string workspaceId, string name, string icon);
    Task<bool> DeleteAsync(string id);
    Task<bool> UpdateAsync(string id, string? newName, string? newIcon);
}
