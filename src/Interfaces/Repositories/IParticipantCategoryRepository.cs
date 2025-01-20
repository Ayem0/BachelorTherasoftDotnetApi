using System;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Repositories;

public interface IParticipantCategoryRepository : IBaseRepository<ParticipantCategory>
{
    Task<List<ParticipantCategory>> GetByWorkpaceIdAsync(string id);
}
