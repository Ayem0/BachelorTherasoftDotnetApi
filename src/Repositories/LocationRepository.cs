using AutoMapper;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Interfaces;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.EntityFrameworkCore;


namespace BachelorTherasoftDotnetApi.src.Repositories;

public class LocationRepository : BaseMySqlRepository<Location>, ILocationRepository
{
    public LocationRepository(MySqlDbContext context) : base(context)
    {
    }
}
