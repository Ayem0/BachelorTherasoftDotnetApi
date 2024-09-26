using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class AddMemberRequest
{
    [Required]
    public required string WorkspaceId { get; set; }
    [Required]
    public required string MemberId  { get; set; }
}
