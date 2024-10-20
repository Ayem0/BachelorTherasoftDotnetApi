using System;

namespace BachelorTherasoftDotnetApi.src.Exceptions;

public class NotFoundException : Exception
{
    public string EntityName { get; set; }
    public string Id { get; set; } 
    public NotFoundException(string entityName, string id)
    {
        EntityName = entityName;
        Id = id;
    }

}
