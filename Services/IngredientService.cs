using first;
using Microsoft.EntityFrameworkCore;

public class IngredientService : IIngredientService
{
    private readonly IIngredientRepository _ingredientRepository;
    private readonly RecipeContext _context;

    public IngredientService(IIngredientRepository ingredientRepository, RecipeContext context){
        _ingredientRepository = ingredientRepository;
        _context = context;
    }

    public async Task<List<Ingredient>> GetAllAsync(){
        return await _ingredientRepository.GetAllAsync();
    }

    public async Task<Ingredient> GetByIdAsync(int id){
        return await _ingredientRepository.GetByIdAsync(id);
    }

  public async Task<Ingredient> AddAsync(Ingredient ingredient)
{
    
    var existingIngredient = await _ingredientRepository.GetByNameAsync(ingredient.Name);
    if (existingIngredient != null)
    {
        return existingIngredient; 
    }

    ingredient.Id = 0; 
    await _ingredientRepository.AddAsync(ingredient);
    return ingredient;
}

    public async Task<Ingredient> UpdateAsync(int id, Ingredient ingredient){
        var existingIngredient = await _ingredientRepository.GetByIdAsync(id);
        
        if (existingIngredient == null)
            return null;
            
        existingIngredient.Name = ingredient.Name;
        
        await _ingredientRepository.UpdateAsync(existingIngredient);
        return existingIngredient;
    }

    public async Task<bool> DeleteAsync(int id) {
        var ingredient = await _ingredientRepository.GetByIdAsync(id);
        if (ingredient == null)
            return false;

    var recipesWithIngredient = await _context.Recipes.Include(r => r.Ingredients).Where(r => r.Ingredients.Any(i => i.Id == id)).ToListAsync();

        foreach (var recipe in recipesWithIngredient)
        {
            recipe.Ingredients.RemoveAll(i => i.Id == id);
        }

        await _context.SaveChangesAsync();
        
        return await _ingredientRepository.DeleteAsync(id);
    }
}