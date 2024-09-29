using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class ParticipantRepository : BaseMySqlRepository<Participant>, IParticipantRepository
{
    public ParticipantRepository(MySqlDbContext context) : base(context)
    {
    }
}
