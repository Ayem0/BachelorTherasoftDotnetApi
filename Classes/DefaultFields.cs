namespace BachelorTherasoftDotnetApi.Classes;

public class DefaultFields
{
    public DateTime CreatedAt = DateTime.Now;
    public DateTime ?ModifiedAt { get; set ; }
    public DateTime ?DeletedAt { get; set ; }
    
}
