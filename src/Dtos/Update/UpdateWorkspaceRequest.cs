using System.ComponentModel.DataAnnotations;

namespace BachelorTherasoftDotnetApi.src.Dtos.Update;
public class UpdateWorkspaceRequest
{
    public string? NewName { get; set; }
    public string? NewDescription { get; set; }
}
