using System;
using BachelorTherasoftDotnetApi.src.Enums;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class EventMemberDto
{
    public EventMemberDto(EventMember eventMember)
    {
        EventId = eventMember.EventId;
        MemberId = eventMember.MemberId;
        Status = eventMember.Status;
    }
    public string EventId { get; set; }
    public string MemberId { get; set; }
    public Status Status { get; set; }
}
