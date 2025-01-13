using System;

namespace BachelorTherasoftDotnetApi.src.Dtos.Update;
public class UpdateParticipantCategoryRequest
{
    public string? NewName { get; set; }
    public string? NewDescription { get; set; }
    public string? NewColor { get; set; }
    public string? NewIcon { get; set;}
}
