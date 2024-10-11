using System;

namespace BachelorTherasoftDotnetApi.src.Base;

public class ErrorResponse<T>
{
    public ErrorResponse(List<string> errors, T content)
    {
        Errors = errors;
        Content = content;
    }
    public List<string> Errors { get; set; }
    public T? Content { get; set; }
}
