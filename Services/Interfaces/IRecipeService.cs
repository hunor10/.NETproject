public interface IRecipeService
{
    Task<List<RecipeDto>> GetAllAsync();
    Task<RecipeDto> GetByIdAsync(int id);
    Task<List<Recipe>> GetByFilterAsync(string name, string ingredient);
    Task<Recipe> AddAsync(Recipe recipe);
    Task<Recipe> UpdateAsync(int id, Recipe recipe);
    Task<bool> DeleteAsync(int id);
    Task<Recipe?> GetRandomRecipeAsync(List<string> availableIngredients);
}