using System;

namespace BachelorTherasoftDotnetApi.src.Base;

public class SuccessResponse
{
    public SuccessResponse(string? message)
    {
        Message = message;
    }
    public string? Message { get; set; }
}
