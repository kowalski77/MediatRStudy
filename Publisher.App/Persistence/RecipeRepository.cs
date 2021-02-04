using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Publisher.App.Application.Infrastructure;
using Publisher.App.Domain;

namespace Publisher.App.Persistence
{
    public sealed class RecipeRepository : IRecipeRepository
    {
        private const string RecipesCollectionName = "Recipes";

        private readonly ISessionHolder sessionHolder;
        private readonly IMongoCollection<Recipe> recipesCollection;
        private readonly IChangeTracker changeTracker;

        public RecipeRepository(
            ISessionHolder sessionHolder,
            IMongoDatabase mongoDatabase, 
            IChangeTracker changeTracker)
        {
            this.sessionHolder = sessionHolder ?? throw new ArgumentNullException(nameof(sessionHolder));
            this.changeTracker = changeTracker ?? throw new ArgumentNullException(nameof(changeTracker));
            if (mongoDatabase == null)
            {
                throw new ArgumentNullException(nameof(mongoDatabase));
            }
            this.recipesCollection = mongoDatabase.GetCollection<Recipe>(RecipesCollectionName);
        }

        public async Task AddRecipeAsync(Recipe recipe)
        {
            await this.recipesCollection.InsertOneAsync(recipe).ConfigureAwait(false);
        }

        public async Task<Recipe> GetRecipeAsync(string name)
        {
            var builder = Builders<Recipe>.Filter;
            var filter = builder.Eq(x => x.Name, name);

            var recipe = (await this.recipesCollection.FindAsync(this.sessionHolder.Session, filter)
                    .ConfigureAwait(false))
                .FirstOrDefault();

            return recipe;
        }

        public async Task SaveAsync(Recipe recipe)
        {
            this.changeTracker.UpdateEntity(recipe);

            await this.recipesCollection.ReplaceOneAsync(this.sessionHolder.Session, x => x.Id == recipe.Id, recipe)
                .ConfigureAwait(false);
        }
    }
}