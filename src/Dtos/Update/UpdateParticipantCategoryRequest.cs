using System;

namespace BachelorTherasoftDotnetApi.src.Dtos.Update;
public class UpdateParticipantCategoryRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Color { get; set; }
    public string? Icon { get; set;}
}
