using BachelorTherasoftDotnetApi.src.Base;

namespace BachelorTherasoftDotnetApi.src.Models;

internal class Conversation : BaseModel
{
    public string ?Name { get; set; }
    public required List<string> MembersId { get; set; }
}

