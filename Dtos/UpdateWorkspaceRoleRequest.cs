using System;
using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.Dtos;

public class UpdateWorkspaceRoleRequest
{
    [Required]
    public required string NewName { get; set; }
}
