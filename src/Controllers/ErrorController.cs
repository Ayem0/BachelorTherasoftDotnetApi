using BachelorTherasoftDotnetApi.src.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpPost("")]
        public IActionResult HandleError()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            var problemDetails = new ProblemDetails
            {
                Status = 500,
                Title = "An unexpected error occurred.",
            };
            switch (context?.Error)
            {
                case NotFoundException notFoundException:
                    problemDetails.Status = StatusCodes.Status404NotFound;
                    problemDetails.Title = $"{notFoundException.EntityName} not found.";
                    problemDetails.Detail = $"{notFoundException.EntityName} with id '{notFoundException.Id}'not found.";
                    break;

                case DbException dbException:
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    problemDetails.Title = "An unexpected error occurred. Try again.";
                    break;
            }

            return StatusCode(problemDetails.Status.Value, problemDetails);
        }
    }
}
