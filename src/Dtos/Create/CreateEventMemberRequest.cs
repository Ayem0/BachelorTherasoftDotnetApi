namespace BachelorTherasoftDotnetApi.src.Dtos.Create;

public class CreateEventMemberRequest
{
    public required string EventId { get; set; }
    public required string MemberId { get; set; }
}
