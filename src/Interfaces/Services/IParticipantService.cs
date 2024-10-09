using System;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos;
using BachelorTherasoftDotnetApi.src.Dtos.Create;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Dtos.Update;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IParticipantService
{
    Task<Response<ParticipantDto?>> GetByIdAsync(string id);
    Task<Response<ParticipantDto?>> CreateAsync(CreateParticipantRequest request);
    Task<Response<string>> DeleteAsync(string id);
    Task<Response<ParticipantDto?>> UpdateAsync(string id, UpdateParticipantRequest request);
}
