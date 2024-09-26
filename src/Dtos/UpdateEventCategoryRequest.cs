using System;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class UpdateEventCategoryRequest
{
    public required string NewName { get; set; }
    public required string NewIcon { get; set; }
}
