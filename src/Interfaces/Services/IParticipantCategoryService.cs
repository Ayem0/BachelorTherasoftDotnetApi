using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IParticipantCategoryService
{
    Task<ParticipantCategoryDto?> GetByIdAsync(string id);
    Task<ParticipantCategoryDto> CreateAsync(CreateParticipantCategoryRequest req);
    Task<bool> DeleteAsync(string id);
    Task<ParticipantCategoryDto> UpdateAsync(string id, UpdateParticipantCategoryRequest req);
    Task<List<ParticipantCategoryDto>> GetByWorkspaceIdAsync(string id);
}
