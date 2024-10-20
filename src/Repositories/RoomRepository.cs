using BachelorTherasoftDotnetApi.src.Models;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Base;
using Microsoft.EntityFrameworkCore;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using AutoMapper;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class RoomRepository : BaseMySqlRepository<Room>, IRoomRepository
{
    public RoomRepository(MySqlDbContext context) : base(context)
    {
    }
}
