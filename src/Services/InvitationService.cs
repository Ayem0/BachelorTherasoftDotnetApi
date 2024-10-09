using System;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;

namespace BachelorTherasoftDotnetApi.src.Services;

public class InvitationService
{
    private readonly IInvitationRepository _invitationRepository;
    public InvitationService(IInvitationRepository _invitationRepository)
    {
        
    }
}
