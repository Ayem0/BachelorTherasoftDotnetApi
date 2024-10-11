using System;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace BachelorTherasoftDotnetApi.src.Services;

public static class ResponseService
{
    public static ActionResult CreateNotFoundResponse(string id, string entityName)
    {
        return new NotFoundObjectResult(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Resource not found",
                Detail = $"{entityName} with ID '{id}' was not found.",
            });
    }

    public static ActionResult CreateBadRequestResponse(string? message, string? details)
    {
        return new BadRequestObjectResult(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = message,
                Detail = details,
            });
    }

    public static ActionResult CreateOkResponse<T>(T entity)
    {
        return new OkObjectResult(entity);
    }

    public static ActionResult CreateCreatedAtResponse<T>(T entity, string controllerName)
    {
        return new CreatedAtActionResult(nameof(entity), controllerName, null, entity);
    }

    public static ActionResult CreateBadRequestResponse(string? message, string? details)
    {
        return new BadRequestObjectResult(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = message,
                Detail = details,
            });
    }
}
