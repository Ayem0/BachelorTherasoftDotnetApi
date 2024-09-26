using System;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class EventCategoryRepository : BaseRepository<EventCategory>
{
     public EventCategoryRepository(MySqlDbContext context) : base(context)
    {
    }
}
