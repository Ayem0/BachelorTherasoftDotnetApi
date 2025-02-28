using System;

namespace BachelorTherasoftDotnetApi.src.Dtos.Create;

public class CreateWorkspaceInvitationRequest
{
    public required string WorkspaceId { get; set; }
    public required string ReceiverUserId { get; set; }
}
