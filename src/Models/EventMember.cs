using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Enums;

namespace BachelorTherasoftDotnetApi.src.Models;

public class EventMember : BaseModel
{
    public EventMember(Member member, Event @event)
    {
        Member = member;
        MemberId = member.Id;
        Event = @event;
        EventId = @event.Id;
        Status = Status.Pending;
    }
    public EventMember(string memberId, string eventId, Status status) // maybe no need du status ici
    {
        MemberId = memberId;
        EventId = eventId;
        Status = status;
    }
    public required Member Member { get; set; }
    public string MemberId { get; set; }
    public required Event Event { get; set; }
    public string EventId { get; set; }
    public Status Status { get; set; }
}
