using BachelorTherasoftDotnetApi.Base;

namespace BachelorTherasoftDotnetApi.Models;

internal class Conversation : BaseModel
{
    public string ?Name { get; set; }
    public required List<string> MembersId { get; set; }
}

