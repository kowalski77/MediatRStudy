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
            recipe.AddIngredient(new Ingredient("ingredient1", 10, IngredientType.AcidRegulator));
            recipe.AddIngredient(new Ingredient("ingredient3", 10, IngredientType.Color));
            recipe.AddIngredient(new Ingredient("ingredient4", 10, IngredientType.Emulsifier));
            recipe.AddIngredient(new Ingredient("ingredient5", 10, IngredientType.AcidRegulator));
            recipe.AddIngredient(new Ingredient("ingredient6", 10, IngredientType.AcidRegulator));
            recipe.AddIngredient(new Ingredient("ingredient7", 10, IngredientType.AcidRegulator));
            recipe.AddIngredient(new Ingredient("ingredient8", 10, IngredientType.AcidRegulator));
            recipe.AddIngredient(new Ingredient("ingredient9", 10, IngredientType.AcidRegulator));
            recipe.AddIngredient(new Ingredient("ingredient10", 10, IngredientType.AcidRegulator));
            recipe.AddIngredient(new Ingredient("ingredient11", 10, IngredientType.AcidRegulator));

            await mongoContext.RecipesCollection.InsertOneAsync(recipe).ConfigureAwait(false);

            var recipe2 = new Recipe("recipe2");
            recipe2.AddIngredient(new Ingredient("ingredient21", 10, IngredientType.AcidRegulator));
            recipe2.AddIngredient(new Ingredient("ingredient22", 10, IngredientType.Emulsifier));
            recipe2.AddIngredient(new Ingredient("ingredient23", 10, IngredientType.Color));
            recipe2.AddIngredient(new Ingredient("ingredient24", 10, IngredientType.Flavor));
            recipe2.AddIngredient(new Ingredient("ingredient25", 10, IngredientType.AcidRegulator));

            await mongoContext.RecipesCollection.InsertOneAsync(recipe2).ConfigureAwait(false);
        }
    }
}