using System.Threading.Tasks;
using first;

public interface IRecipeRepository
{
    Task<List<Recipe>> GetAllAsync();
    Task<Recipe> GetByIdAsync(int id);
    Task<List<Recipe>> GetByFilterAsync(string name, string ingredient);
    Task AddAsync(Recipe recipe);
    Task<Recipe> UpdateAsync(Recipe recipe);
    Task<bool> DeleteAsync(int id);
    Task<Recipe?> GetRandomRecipeAsync(List<string> availableIngredients); //extra funkcionalitas

}