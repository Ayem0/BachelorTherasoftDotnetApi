using System.ComponentModel.DataAnnotations;
using BachelorTherasoftDotnetApi.src.Models;

namespace BachelorTherasoftDotnetApi.src.Dtos.Models;

public class UserDto
{
    public UserDto(User user)
    {
        Id = user.Id;
        FirstName = user.FirstName;
        LastName = user.LastName;
    }
    [Required]
    public string Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
