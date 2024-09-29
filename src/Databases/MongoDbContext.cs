using BachelorTherasoftDotnetApi.src.Models;
using MongoDB.Driver;

namespace BachelorTherasoftDotnetApi.src.Databases;

public class MongoDbContext
{
    public readonly IMongoDatabase _database;
    public MongoDbContext(IConfiguration configuration)
    {
        var connexionString = configuration.GetConnectionString("MongoDB");
        var client = new MongoClient(connexionString);
        _database = client.GetDatabase(configuration["MongoDbSettings:DbName"]);
    }
    public IMongoCollection<Conversation> Conversations => _database.GetCollection<Conversation>("Conversations");
    public IMongoCollection<Message> Messages => _database.GetCollection<Message>("Messages");
}
