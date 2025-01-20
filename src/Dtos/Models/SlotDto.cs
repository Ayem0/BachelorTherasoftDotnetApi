using System;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos.Models;

public class SlotDto
{
    public string Id { get; set; } = string.Empty;
    public string WorkspaceId { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    
}

public class SlotWithEventCategoriesDto : SlotDto
{    
    public List<EventCategoryDto> EventCategories { get; set; } = [];
}
