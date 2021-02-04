using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Publisher.App.Domain;
using Publisher.App.Persistence;

namespace Publisher.App
{
    public static class StartupExtensions
    {
        public static async Task SeedDatabaseAsync(this IServiceProvider serviceProvider)
        {
            var mongoContext = serviceProvider.GetService<MongoDbContext>();
            await mongoContext.RecipesCollection.Database.DropCollectionAsync("Recipes").ConfigureAwait(false);

            var recipe = new Recipe("recipe1");
            recipe.AddIngredient(new Ingredient("ingredient1", 11, IngredientType.AcidRegulator));
            recipe.AddIngredient(new Ingredient("ingredient2", 12, IngredientType.AntiCakingAgent));
            recipe.AddIngredient(new Ingredient("ingredient3", 13, IngredientType.Color));
            recipe.AddIngredient(new Ingredient("ingredient4", 14, IngredientType.AcidRegulator));
            recipe.AddIngredient(new Ingredient("ingredient5", 15, IngredientType.AcidRegulator));

            await mongoContext.RecipesCollection.InsertOneAsync(recipe).ConfigureAwait(false);

            var recipe2 = new Recipe("recipe2");
            recipe2.AddIngredient(new Ingredient("ingredient21", 21, IngredientType.AcidRegulator));
            recipe2.AddIngredient(new Ingredient("ingredient22", 22, IngredientType.Emulsifier));
            recipe2.AddIngredient(new Ingredient("ingredient23", 23, IngredientType.Color));
            recipe2.AddIngredient(new Ingredient("ingredient24", 24, IngredientType.Flavor));
            recipe2.AddIngredient(new Ingredient("ingredient25", 25, IngredientType.AcidRegulator));

            await mongoContext.RecipesCollection.InsertOneAsync(recipe2).ConfigureAwait(false);
        }
    }
}