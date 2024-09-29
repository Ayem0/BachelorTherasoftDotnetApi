using System;
using BachelorTherasoftDotnetApi.src.Databases;
using MongoDB.Driver;

namespace BachelorTherasoftDotnetApi.src.Base;

public class BaseMongoDbRepository<T> : IBaseRepository<T> where T : BaseModel
{
    protected readonly IMongoCollection<T> collection;
    public BaseMongoDbRepository(MongoDbContext mongoDbContext)
    {
        collection = mongoDbContext._database.GetCollection<T>(typeof(T).Name);
    }
    public async Task CreateAsync(T entity)
    {
        entity.CreatedAt = DateTime.Now;
        await collection.InsertOneAsync(entity);
    }

    public async Task DeleteAsync(T entity)
    {
        entity.DeletedAt = DateTime.Now;
        var filter = Builders<T>.Filter.Eq(doc => doc.Id, entity.Id);
        await collection.ReplaceOneAsync(filter, entity);
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        var filter = Builders<T>.Filter.And(
            Builders<T>.Filter.Eq(doc => doc.Id, id),
            Builders<T>.Filter.Eq(doc => doc.DeletedAt, null)
        );
        return await collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        entity.UpdatedAt = DateTime.Now;
        var filter = Builders<T>.Filter.Eq(doc => doc.Id, entity.Id);
        await collection.ReplaceOneAsync(filter, entity);
    }
}
