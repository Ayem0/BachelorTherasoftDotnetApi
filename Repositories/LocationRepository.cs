using BachelorTherasoftDotnetApi.Base;
using BachelorTherasoftDotnetApi.Databases;
using BachelorTherasoftDotnetApi.Interfaces;
using BachelorTherasoftDotnetApi.Models;


namespace BachelorTherasoftDotnetApi.Repositories;

public class LocationRepository : BaseRepository<Location>, ILocationRepository
{
    public LocationRepository(MySqlDbContext context) : base(context)
    {
    }
}
