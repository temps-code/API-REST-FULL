using MongoDB.Driver;
using prueba1.Models;

namespace prueba1
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
            _database = client.GetDatabase(configuration["MongoDbDatabase"]);
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
    }
}
