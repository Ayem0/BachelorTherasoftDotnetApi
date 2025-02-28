using System;
using BachelorTherasoftDotnetApi.src.Enums;

namespace BachelorTherasoftDotnetApi.src.Dtos.Create;

public class CreateContactInvitationRequest
{
    public required string ContactEmail { get; set; }
}
