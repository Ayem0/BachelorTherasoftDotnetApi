using System;
using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Interfaces.Repositories;

public interface IInvitationRepository : IBaseRepository<Invitation>
{
    Task<List<Invitation>> GetByReceiverUserIdAsync(string userId);
}
