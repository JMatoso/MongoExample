using Customers.API.Entities;
using Customers.API.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Customers.API.Repository
{
    public class MockEntity
    {
        private readonly IMongoCollection<Customer> _customersCollection;

        public MockEntity(IOptions<CustomersDatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

            _customersCollection = mongoDatabase.GetCollection<Customer>(databaseSettings.Value.CollectionName);
        }

        public async Task<IEnumerable<Customer>> GetAsync() =>
            await _customersCollection.Find(_ => true).ToListAsync();

        public async Task<Customer?> GetAsync(Guid id) =>
            await _customersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(params Customer[] newCustomers) =>
            await _customersCollection.InsertManyAsync(newCustomers);

        public async Task UpdateAsync(Guid id, Customer updateCustomer) =>
            await _customersCollection.ReplaceOneAsync(x => x.Id == id, updateCustomer);

        public async Task RemoveAsync(Guid id) =>
            await _customersCollection.DeleteOneAsync(x => x.Id == id);
    }
}
