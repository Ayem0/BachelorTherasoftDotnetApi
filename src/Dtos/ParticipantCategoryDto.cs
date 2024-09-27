using System;
using System.ComponentModel.DataAnnotations;
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
    [Required]
    public string Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Icon { get; set; }
}
