using System;
using BachelorTherasoftDotnetApi.src.Enums;

namespace BachelorTherasoftDotnetApi.src.Dtos.Models;

public class EventUserDto : UserDto
{
    public Status Status { get; set; }
}
