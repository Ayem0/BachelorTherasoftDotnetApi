using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IParticipantService
{
    Task<ParticipantDto?> GetByIdAsync(string workspaceId, string id);
    Task<ParticipantJoinCategoryDto> CreateAsync(string workspaceId, CreateParticipantRequest request);
    Task<bool> DeleteAsync(string workspaceId, string id);
    Task<ParticipantJoinCategoryDto> UpdateAsync(string workspaceId, string id, UpdateParticipantRequest request);
    Task<List<ParticipantDto>> GetByWorkspaceIdAsync(string id);
    Task<List<ParticipantJoinCategoryDto>> GetByWorkspaceIdJoinCategoryAsync(string id);
}
