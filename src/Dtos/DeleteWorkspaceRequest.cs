using System;
using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.src.Dtos;

public class DeleteWorkspaceRequest
{
    [Required]
    public required string Id { get; set;}
}
