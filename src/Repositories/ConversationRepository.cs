// using BachelorTherasoftDotnetApi.src.Base;
// using BachelorTherasoftDotnetApi.src.Databases;
// using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
// using BachelorTherasoftDotnetApi.src.Models;
// using Microsoft.EntityFrameworkCore;
// using MongoDB.Driver;

// namespace BachelorTherasoftDotnetApi.src.Repositories;

// public class ConversationRepository : BaseMongoDbRepository<Conversation>, IConversationRepository
// {
//     public ConversationRepository(MongoDbContext context) : base(context)
//     {
//     }
//     public async Task<List<Conversation>> GetByUserIdAsync(string id)
//     {

//         return await _context.Conversation.Where(c => c.DeletedAt == null && c.UserIds.All(u => u == id)).ToListAsync();
//     }
// }
