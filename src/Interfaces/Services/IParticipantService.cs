using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IParticipantService
{
    Task<ActionResult<ParticipantDto>> GetByIdAsync(string id);
    Task<ActionResult<ParticipantDto>> CreateAsync(CreateParticipantRequest request);
    Task<ActionResult> DeleteAsync(string id);
    Task<ActionResult<ParticipantDto>> UpdateAsync(string id, UpdateParticipantRequest request);
}
