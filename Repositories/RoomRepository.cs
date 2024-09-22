using System;
using BachelorTherasoftDotnetApi.Models;
using BachelorTherasoftDotnetApi.Interfaces;
using BachelorTherasoftDotnetApi.Databases;
using BachelorTherasoftDotnetApi.Base;

namespace BachelorTherasoftDotnetApi.Repositories;

public class RoomRepository : BaseRepository<Location>, ILocationRepository
{
     public RoomRepository(MySqlDbContext context) : base(context)
    {
    }
}
