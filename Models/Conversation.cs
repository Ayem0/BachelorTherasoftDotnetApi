using BachelorTherasoftDotnetApi.Classes;

namespace BachelorTherasoftDotnetApi.Models;

internal class Conversation : DefaultFields
{
    public string Id = Guid.NewGuid().ToString();
    public string ?Name { get; set; }
    public required List<string> MembersId { get; set; }
}

