using System;

namespace BachelorTherasoftDotnetApi.src.Base;

public class Response<T>
{
    public Response(T content, bool success)
    {
        Success = success;
        Content = content;
        Errors = null;
    }

    public Response(List<string> errors, bool success)
    {
        Success = success;
        Errors = errors;
        Content = default;
    }
    public required bool Success { get; set; }
    public List<string>? Errors { get; set; }
    public T? Content { get; set; }
}
