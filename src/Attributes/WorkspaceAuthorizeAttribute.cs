// using BachelorTherasoftDotnetApi.src.Base;
// using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
// using BachelorTherasoftDotnetApi.src.Interfaces.Services;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Filters;
// using System.Security.Claims;

// public class WorkspaceAuthorizeAttribute : Attribute, IAsyncActionFilter
// {
//     private readonly string _tableName;
//     public WorkspaceAuthorizeAttribute(string tableName)
//     {
//         _tableName = tableName;
//     }
//     public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
//     {
//         string? userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//         if (string.IsNullOrEmpty(userId))
//         {
//             context.Result = new UnauthorizedResult();
//             return;
//         }

//         if (!context.ActionArguments.TryGetValue("id", out var id) || id == null)
//         {
//             context.Result = new BadRequestObjectResult("Id param is required.");
//             return;
//         }

//         string entityId = id.ToString()!;

//         var userService = context.HttpContext.RequestServices.GetService<IUserService>();
//         if (userService == null)
//         {
//             context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
//             return;
//         }

//         var workspaceAuthorizationService = context.HttpContext.RequestServices.GetService<IWorkspaceAuthorizationService>();
//         if (workspaceAuthorizationService == null)
//         {
//             context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
//             return;
//         }

//         var entity = await workspaceAuthorizationService.GetEntityById(_tableName, entityId);
//         if (entity == null) {
//             context.Result = new ForbidResult();
//             return;
//         }

//         var user = await userService.GetUserJoinWorkspacesByIdAsync(userId);
//         if (user == null || user.Workspaces == null || !user.Workspaces.Any(ws => _tableName == "Workspace" ? ws.Id ==  entity.Id : ws.Id == entity.WorkspaceId))
//         {
//             context.Result = new ForbidResult();
//             return;
//         }

//         // TODO check user right with user request
//         await next();
//     }
// }
