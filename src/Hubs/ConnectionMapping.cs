// using System;

// namespace BachelorTherasoftDotnetApi.src.Hubs;

// public class ConnectionMapping {
//     private readonly Dictionary<string, HashSet<string>> _connections = [];

//     public void Add(string key, string connectionId)
//     {
//         lock (_connections)
//         {
//             if (!_connections.TryGetValue(key, out var connections))
//             {
//                 connections = [];
//                 _connections[key] = connections;
//             }
//             connections.Add(connectionId);
//         }
//     }

//     public IEnumerable<string> GetConnections(string key)
//     {
//         lock (_connections)
//         {
//             if (_connections.TryGetValue(key, out var connections))
//             {
//                 return connections;
//             }
//         }
//         return [];
//     }

//     public void Remove(string key, string connectionId)
//     {
//         lock (_connections)
//         {
//             if (!_connections.TryGetValue(key, out var connections)) return;

//             connections.Remove(connectionId);

//             if (connections.Count == 0)
//             {
//                 _connections.Remove(key);
//             }
//         }
//     }
// }

