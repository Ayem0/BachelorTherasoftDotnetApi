using System;
using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class CreateWorkspaceRequest
{
    [Required]
    public required string Name { get; set; }
}
