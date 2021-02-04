using System.Threading.Tasks;

namespace Publisher.App.Domain
{
    public interface IRecipeRepository
    {
        Task AddRecipeAsync(Recipe recipe);

        Task<Recipe> GetRecipeAsync(string name);

        Task SaveAsync(Recipe recipe);
    }
}