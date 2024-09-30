using System;
using BachelorTherasoftDotnetApi.src.Enums;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class UpdateEventMemberRequest
{
    public Status NewStatus { get; set; }
}
