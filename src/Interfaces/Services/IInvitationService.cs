using System;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Services;

public interface IInvitationService
{
    Task<Response<InvitationDto?>> GetByIdAsync(string id);
    Task<Response<string>> DeleteAsync(string id);
    Task<Response<InvitationDto?>> CreateAsync();

}
