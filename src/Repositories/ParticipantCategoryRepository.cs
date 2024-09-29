using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class ParticipantCategoryRepository : BaseMySqlRepository<ParticipantCategory>, IParticipantCategoryRepository
{
    public ParticipantCategoryRepository(MySqlDbContext context) : base(context)
    {
    }
}
