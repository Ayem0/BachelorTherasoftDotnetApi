using BachelorTherasoftDotnetApi.src.Enums;

namespace BachelorTherasoftDotnetApi.src.Exceptions;

public class DbException : Exception
{
    public DbAction Action { get; set; }
    public string EntityName { get; set; }
    public string? Id { get; set; }
    public DbException(DbAction action, string entityName, string id)
    {
        Action = action;
        EntityName = entityName;
        Id = id;
    }

    public DbException(DbAction action, string entityName)
    {
        Action = action;
        EntityName = entityName;
    }
}
