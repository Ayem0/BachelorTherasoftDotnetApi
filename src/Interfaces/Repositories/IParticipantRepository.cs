using System;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Repositories;

public interface IParticipantRepository : IBaseRepository<Participant>
{
    Task<List<Participant>> GetByWorkpaceIdAsync(string id);
    Task<List<Participant>> GetByWorkpaceIdJoinCategoryAsync(string id);
}
