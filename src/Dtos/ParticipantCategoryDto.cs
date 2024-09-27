using System;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class ParticipantCategoryDto
{
    public ParticipantCategoryDto(ParticipantCategory participantCategory)
    {
        Id = participantCategory.Id;
        Name = participantCategory.Name;
        Icon = participantCategory.Icon;
    }
    public string Id { get; set; }
    public string Name { get; set; }
    public string Icon { get; set; }
}
