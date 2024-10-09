using System;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class InvitationRepository : BaseMongoDbRepository<Invitation>, IInvitationRepository
{
    public InvitationRepository(MongoDbContext context) : base(context)
    {
    }
    public async Task<List<Invitation>> GetByReceiverUserIdAsync(string userId)
    {
        return await _context.Invitation.Where(x => x.ReceiverUserId == userId).ToListAsync();
    }
}
