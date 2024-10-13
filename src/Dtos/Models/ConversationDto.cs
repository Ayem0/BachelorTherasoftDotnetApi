using System;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos.Models;

public class ConversationDto
{
    public string Id { get; set;} = string.Empty;
    public string? Name { get; set;}
}

public class ConversationWithUsersDto : ConversationDto
{
    public List<UserDto> Users { get; set;} = [];
}
