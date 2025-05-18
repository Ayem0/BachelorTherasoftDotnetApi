// using BachelorTherasoftDotnetApi.src.Models;

// namespace BachelorTherasoftDotnetApi.src.Dtos.Models;

using System.ComponentModel.DataAnnotations;

public class MemberDto
{
    [Required]
    public string Id { get; set; } = string.Empty;
    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;
}
