using System;

namespace BachelorTherasoftDotnetApi.src.Utils;

public class DbResponse<T> where T : class
{
    public DbResponse()
    {
        Success = true;
    }

    public DbResponse(T? data)
    {
        Success = true;
        Data = data;
    }
    public DbResponse(string message)
    {
        Success = false;
        Message = message;
    }
    public DbResponse(string message, string details)
    {
        Success = false;
        Message = message;
        Details = details;
    }
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public string? Details { get; set; }
}
