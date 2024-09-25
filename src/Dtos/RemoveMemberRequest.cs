using System;
using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class RemoveMemberRequest
{
    [Required]
    public required string WorkspaceId { get; set; }
    [Required]
    public required string MemberId  { get; set; }
}
