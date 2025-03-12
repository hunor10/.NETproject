
public interface IIngredientRepository
{
    Task<List<Ingredient>> GetAllAsync();
    Task<Ingredient> GetByIdAsync(int id);
    Task<Ingredient> GetByNameAsync(string name);
    Task AddAsync(Ingredient ingredient);
    Task<Ingredient> UpdateAsync(Ingredient ingredient);
    Task<bool> DeleteAsync(int id);
    
}