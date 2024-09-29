using BachelorTherasoftDotnetApi.src.Base;
using BachelorTherasoftDotnetApi.src.Databases;
using BachelorTherasoftDotnetApi.src.Interfaces.Repositories;
using BachelorTherasoftDotnetApi.src.Models;
using MongoDB.Driver;

namespace BachelorTherasoftDotnetApi.src.Repositories;

public class ConversationRepository : BaseMongoDbRepository<Conversation>, IConversationRepository
{
    public ConversationRepository(MongoDbContext context) : base(context)
    {
    }
    public async Task<List<Conversation>> GetByUserIdAsync(string id)
    {
        var filter = Builders<Conversation>.Filter.And(
            Builders<Conversation>.Filter.AnyEq(doc => doc.UserIds, id),
            Builders<Conversation>.Filter.Eq(doc => doc.DeletedAt, null)
        );
        return await collection.Find(filter).ToListAsync();
    }
}
