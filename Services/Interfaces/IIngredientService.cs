public interface IIngredientService
{
    Task<List<Ingredient>> GetAllAsync();
    Task<Ingredient> GetByIdAsync(int id);
    Task<Ingredient> AddAsync(Ingredient ingredient);
    Task<Ingredient> UpdateAsync(int id, Ingredient ingredient);
    Task<bool> DeleteAsync(int id);
}