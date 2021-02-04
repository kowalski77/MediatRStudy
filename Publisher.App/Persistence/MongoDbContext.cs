using MongoDB.Driver;
using Publisher.App.Domain;

namespace Publisher.App.Persistence
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase database;

        public MongoDbContext()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            this.database = client.GetDatabase("MediatRStudyDB");
        }

        public IMongoCollection<Recipe> RecipesCollection => this.database.GetCollection<Recipe>("Recipes");
    }
}