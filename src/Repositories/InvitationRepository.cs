using System;
using AutoMapper;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class InvitationRepository : BaseMySqlRepository<Invitation>, IInvitationRepository
{
    public InvitationRepository(MySqlDbContext context, IMapper mapper) : base(context, mapper)
    {
    }
    public async Task<List<Invitation>> GetByReceiverUserIdAsync(string userId)
    {
        return await _context.Invitatition.Where(x => x.ReceiverId == userId).ToListAsync();
    }
}
