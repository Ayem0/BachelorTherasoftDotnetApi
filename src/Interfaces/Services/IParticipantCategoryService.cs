using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IParticipantCategoryService
{
    Task<ParticipantCategoryDto?> GetByIdAsync(string workspaceId, string id);
    Task<ParticipantCategoryDto> CreateAsync(string workspaceId, CreateParticipantCategoryRequest req);
    Task<bool> DeleteAsync(string workspaceId, string id);
    Task<ParticipantCategoryDto> UpdateAsync(string workspaceId, string id, UpdateParticipantCategoryRequest req);
    Task<List<ParticipantCategoryDto>> GetByWorkspaceIdAsync(string id);
}
