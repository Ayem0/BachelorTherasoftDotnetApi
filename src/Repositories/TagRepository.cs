using System;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class TagRepository : BaseMySqlRepository<Tag>, ITagRepository
{
    public TagRepository(MySqlDbContext context) : base(context)
    {
    }
}
