// using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.SignalR;

// namespace BachelorTherasoftDotnetApi.src.Hubs;
// [Authorize]
// public class WorkspaceHub : Hub
// {
//     // private static readonly ConnectionMapping _connections = new();
//     private readonly IUserRepository _userRepository;
//     public WorkspaceHub(IUserRepository userRepository)
//     {
//         _userRepository = userRepository;
//     }

//         public override async Task<Task> OnConnectedAsync()
//         {
//             var userId = Context.UserIdentifier;

//             if (userId != null)
//             {
//                 var user = await _userRepository.GetByIdJoinWorkspaceAsync(userId);
//                 if (user != null) {
//                     foreach (var workspace in user.Workspaces) 
//                     {
//                         await Groups.AddToGroupAsync(Context.ConnectionId, workspace.Id);
//                     }
//                 }
//                 _connections.Add(userId, Context.ConnectionId);
//             }
//             return base.OnConnectedAsync();
//         }

//         public override async Task<Task> OnDisconnectedAsync(Exception? exception)
//         {
//             var userId = Context.UserIdentifier; 
//             if (userId != null)
//             {
//                 var user = await _userRepository.GetByIdAsync(userId);
//                 if (user != null) {
//                     foreach (var workspace in user.Workspaces) 
//                     {
//                         await Groups.RemoveFromGroupAsync(Context.ConnectionId, workspace.Id);
//                     }
//                 }
//                 _connections.Remove(userId, Context.ConnectionId);
//             }
//             return base.OnDisconnectedAsync(exception);
//         }

//     public async Task NotifyWorkspaceGroup(string workspaceId, string message) 
//     {
//         await Clients.Group(workspaceId).SendAsync("ReceiveNotification", message);
//         Console.WriteLine("NOTIFIED WORKSPACE GROUP USERS");
//     }
// }