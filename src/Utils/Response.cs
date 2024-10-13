using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Utils;

public static class Response
{
    public static NotFoundObjectResult NotFound(string id, string entityName)
    {
        return new NotFoundObjectResult(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = $"{entityName} not found.",
                Detail = $"{entityName} with ID '{id}' was not found.",
            });
    }

    public static BadRequestObjectResult BadRequest(string? title, string? details)
    {
        return new BadRequestObjectResult(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = title,
                Detail = details,
            });
    }

    public static OkObjectResult Ok<T>(T entity)
    {
        return new OkObjectResult(entity);
    }

    public static CreatedAtActionResult CreatedAt<T>(T entity)
    {
        return new CreatedAtActionResult(null, null, null, entity);
    }

    public static NoContentResult NoContent()
    {
        return new NoContentResult();
    }

    public static UnauthorizedResult Unauthorized()
    {
        return new UnauthorizedResult();
    }
}
