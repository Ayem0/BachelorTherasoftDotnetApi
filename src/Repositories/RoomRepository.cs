using System;
using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Interfaces;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Base;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class RoomRepository : BaseRepository<Location>
{
     public RoomRepository(MySqlDbContext context) : base(context)
    {
    }
}
