using Microsoft.EntityFrameworkCore;
using first;

public class IngredientRepository : IIngredientRepository {
    private readonly RecipeContext _context; 

    public IngredientRepository(RecipeContext context){
        _context=context;
    }

    public async Task<List<Ingredient>> GetAllAsync(){
        return await _context.Ingredients.ToListAsync();
    }

    public async Task<Ingredient> GetByIdAsync(int id){
        return await _context.Ingredients.FindAsync(id);
    }

    public async Task<Ingredient> GetByNameAsync(string name){
        return await _context.Ingredients.FirstOrDefaultAsync(i=>i.Name.ToLower()==name.ToLower()); //cmp fg
    }

    public async Task AddAsync(Ingredient ingredient)
{
    ingredient.Id = 0; // Reset Id to let the database generate it
    await _context.Ingredients.AddAsync(ingredient);
    await _context.SaveChangesAsync();    
}

    public async Task<Ingredient> UpdateAsync(Ingredient ingredient){
         var savedIngredient = await _context.Ingredients.FindAsync(ingredient.Id);
    
         if (savedIngredient == null) 
             throw new KeyNotFoundException("The ingredient with the given ID was not found");

        savedIngredient.Name = ingredient.Name;
        savedIngredient.Recipes = ingredient.Recipes;

        await _context.SaveChangesAsync();

        return savedIngredient;
    }

    public async Task<bool> DeleteAsync(int id){
        var ingredient=await _context.Ingredients.FindAsync(id);
        if(ingredient==null) return false; //throw exception
        _context.Ingredients.Remove(ingredient);
        await _context.SaveChangesAsync();
        return true;
    }
}