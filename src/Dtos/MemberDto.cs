using System;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class MemberDto
{
    public MemberDto(Member member)
    {
        UserId = member.UserId;
        WorkspaceId = member.WorkspaceId;
        Status = member.Status;
        Id = member.Id;
    }
    public string Id { get; set; }
    public string UserId { get; set; }
    public string WorkspaceId { get; set; }
    public Status Status { get; set; }
    
}
