using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IParticipantService
{
    Task<ParticipantDto> GetByIdAsync(string id);
    Task<ParticipantDto> CreateAsync(CreateParticipantRequest request);
    Task<bool> DeleteAsync(string id);
    Task<ParticipantDto> UpdateAsync(string id, UpdateParticipantRequest request);
    Task<List<ParticipantDto>> GetByWorkspaceIdAsync(string id);
}
