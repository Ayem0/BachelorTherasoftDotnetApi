using System;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class InvitationRepository : BaseMongoDbRepository<Invitation>, IInvitationRepository
{
    public InvitationRepository(MongoDbContext context) : base(context)
    {
    }
}
