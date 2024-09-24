using System;
using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.Dtos;

public class CreateWorkspaceRoleRequest
{
    [Required]
    public required string Name { get ;set; }
    [Required]
    public required string WorkspaceId { get ;set; }
}
