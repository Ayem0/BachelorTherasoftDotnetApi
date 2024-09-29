using BachelorTherasoftDotnetApi.src.Models;
using Microsoft.EntityFrameworkCore;

namespace BachelorTherasoftDotnetApi.src.Databases;

public class MongoDbContext : DbContext
{
    public MongoDbContext(DbContextOptions<MongoDbContext> options) : base(options)
    {
    }
    public DbSet<Conversation> Conversation { get; set; }

    // public IMongoCollection<Conversation> Conversations => _database.GetCollection<Conversation>("Conversations");
    // public IMongoCollection<Message> Messages => _database.GetCollection<Message>("Messages");
}
