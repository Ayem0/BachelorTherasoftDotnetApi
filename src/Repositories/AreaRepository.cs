using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Interfaces;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Base;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class AreaRepository : BaseRepository<Area>
{
    public AreaRepository(MySqlDbContext context) : base(context)
    {
    }
}
