using System;
using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.Dtos;

public class WorkspaceRoleDto
{
    [Required]
    public required string Id { get; set; }
    [Required]
    public required string Name { get; set; }
}
