using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.src.Dtos.Update;
public class UpdateWorkspaceRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}
