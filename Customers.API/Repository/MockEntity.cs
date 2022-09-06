using MongoDB.Driver;
using Customers.API.Options;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace Customers.API.Repository;

public class MockEntity<TEntity> : IMockEntity<TEntity> where TEntity : class
{
    private readonly IMongoCollection<TEntity> _collection;

    public MockEntity(IOptions<DatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

        _collection = mongoDatabase.GetCollection<TEntity>(databaseSettings.Value.CollectionName);
    }

    public async Task<IEnumerable<TEntity>> GetAsync() =>
        await _collection.Find(_ => true).ToListAsync();

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression) =>
        await _collection.Find(expression).FirstOrDefaultAsync();

    public async Task CreateAsync(params TEntity[] newEntities) =>
        await _collection.InsertManyAsync(newEntities);

    public async Task<bool> UpdateAsync(Expression<Func<TEntity, bool>> expression, TEntity updateCustomer)
    {
        var updateResult = await _collection.ReplaceOneAsync(expression, updateCustomer);

        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }


    public async Task<bool> RemoveAsync(Expression<Func<TEntity, bool>> expression)
    {
        var deleteResult = await _collection.DeleteOneAsync(expression);

        return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }
}
