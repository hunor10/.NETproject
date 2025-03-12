using Microsoft.EntityFrameworkCore;
using first;

public class RecipeRepository : IRecipeRepository {
    private readonly RecipeContext _context;
private readonly Random _random = new Random();

    public RecipeRepository(RecipeContext context)
    {
        _context=context;
    }

    public async Task<List<Recipe>> GetAllAsync(){
        return await _context.Recipes.Include(r=>r.Ingredients).ToListAsync();
    }

    public async Task<Recipe> GetByIdAsync(int id){
    return await _context.Recipes.Include(r => r.Ingredients).FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<List<Recipe>> GetByFilterAsync(string name,string ingredient){
        var query=_context.Recipes.Include(r=>r.Ingredients).AsQueryable();
    
    if(!string.IsNullOrEmpty(name)){ //name helyett beszedes nev
        query=query.Where(r=>r.Name.Contains(name));
    }
    if(!string.IsNullOrEmpty(ingredient)){
         query = query.Where(r => r.Ingredients.Any(i => i.Name.Contains(ingredient)));
    }
    return await query.ToListAsync();
    }

    public async Task AddAsync(Recipe recipe){
        await _context.Recipes.AddAsync(recipe);
        await _context.SaveChangesAsync();
    }


    public async Task<Recipe> UpdateAsync(Recipe recipe){
        var savedRecipe = await _context.Recipes.FindAsync(recipe.Id);
        if(savedRecipe==null) throw new KeyNotFoundException("THe recipe id was not found");
        savedRecipe.Name = recipe.Name;
        savedRecipe.Description = recipe.Description;
        savedRecipe.ImageUrl = recipe.ImageUrl;
        savedRecipe.Ingredients = recipe.Ingredients;

        await _context.SaveChangesAsync();
        return savedRecipe;
    }
    public async Task<bool> DeleteAsync(int id){
        var recipe = await _context.Recipes.FindAsync(id);
        if (recipe == null)
            return false; //try catch new excp
        _context.Recipes.Remove(recipe);
        await _context.SaveChangesAsync();
        return true;
    }
    //extra funkcionalitas: visszaterit egy random receptet az adatbazisbol a hozzavalok alapjan
    public async Task<Recipe?> GetRandomRecipeAsync(List<string> availableIngredients){
        //lekerjuk az osszes receptet s hozzavaloikat
        var recipes = await _context.Recipes.Include(r => r.Ingredients).ToListAsync();
        //kiszurjuk azokat a recepteket amelyek tartalmaznak legalabb egy hozzavalot az availableIngredients-bol
var matchingRecipes = recipes.Where(r => r.Ingredients.Any(i => availableIngredients.Contains(i.Name))).ToList();
//ha talalunk olyan receptet amely megfelel a fennebb emlitett szuresi feltetelnek->veletlenszeruen valasztunk egyet a talaltak kozul
if (matchingRecipes.Any()) return matchingRecipes[_random.Next(matchingRecipes.Count)];
        else if (recipes.Any()) return recipes[_random.Next(recipes.Count)]; else return null;       
    }//kulonben veletlenszeruen valasztunk egyet az osszes recept kozul, illetve ha egy receptet sem talalunk akkor return null
}