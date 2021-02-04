using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Publisher.App.Crosscutting.DomainUtils;
using Publisher.App.Domain;
using Publisher.App.Persistence;

namespace Publisher.App
{
    public static class ApplicationPersistenceExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddSingleton<MongoDbContext>();
            var client = new MongoClient(GetMongoUrl());
            services.AddSingleton(client.GetDatabase("MediatRStudyDB"));

            services.AddScoped<ISessionHolder, SessionHolder>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRecipeRepository, RecipeRepository>();

            MapMongoDbClasses();

            return services;
        }

        private static MongoUrl GetMongoUrl()
        {
            var builder = new MongoUrlBuilder
            {
                Server = new MongoServerAddress("localhost", 27017),
                ReplicaSetName = "rs0"
            };

            return builder.ToMongoUrl();
        }

        private static void MapMongoDbClasses()
        {
            BsonClassMap.RegisterClassMap<Recipe>(cm =>
            {
                cm.AutoMap();
                cm.MapField("ingredients").SetElementName(nameof(Recipe.Ingredients));
            });
        }
    }
}