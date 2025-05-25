using System;
using BachelorTherasoftDotnetApi.src.Dtos.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class DialogEventDto
{
    public List<EventDto> RoomEvents = [];
    public Dictionary<string, List<EventDto>> UsersEvents = [];
}
