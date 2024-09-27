using System;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Interfaces;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class ParticipantCategoryRepository : BaseRepository<ParticipantCategory>, IParticipantCategoryRepository
{
    public ParticipantCategoryRepository(MySqlDbContext context) : base(context)
    {
    }
}
