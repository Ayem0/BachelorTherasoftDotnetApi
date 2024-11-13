using System;

namespace BachelorTherasoftDotnetApi.src.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string title, string details)
    {
        Title = title;
        Details = details;
    }
    public string Title { get; set; }
    public string Details { get; set; }
}
