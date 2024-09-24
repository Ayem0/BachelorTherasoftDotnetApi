using System;
using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.Dtos;

public class CreateWorkspaceRequest
{
    [Required]
    public required string Name { get; set; }
}
