using System.ComponentModel.DataAnnotations;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos.Models;

public class UserDto
{
    [Required]
    public string Id { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
