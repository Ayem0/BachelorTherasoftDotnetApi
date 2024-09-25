using System;
using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class UpdateWorkspaceRoleRequest
{
    [Required]
    public required string NewName { get; set; }
}
