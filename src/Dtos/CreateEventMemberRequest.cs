using System;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class CreateEventMemberRequest
{
    public required string EventId { get; set; }
    public required string MemberId { get; set; }
}
