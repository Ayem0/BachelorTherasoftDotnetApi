using System;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Interfaces;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class ParticipantRepository : BaseRepository<Participant>, IParticipantRepository
{
    public ParticipantRepository(MySqlDbContext context) : base(context)
    {
    }
}
