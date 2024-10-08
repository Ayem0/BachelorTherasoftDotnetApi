using System;
using BachelorTherasoftDotnetApi.src.Enums;

namespace BachelorTherasoftDotnetApi.src.Dtos.Update;
public class UpdateMemberRequest
{
    public Status? NewStatus { get; set; }
}
