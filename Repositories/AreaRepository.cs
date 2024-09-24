using BachelorTherasoftDotnetApi.Models;
using BachelorTherasoftDotnetApi.Interfaces;
using BachelorTherasoftDotnetApi.Databases;
using BachelorTherasoftDotnetApi.Base;

namespace BachelorTherasoftDotnetApi.Repositories;

public class AreaRepository : BaseRepository<Area>
{
    public AreaRepository(MySqlDbContext context) : base(context)
    {
    }
}
