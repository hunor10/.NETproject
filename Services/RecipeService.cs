using first;
using Microsoft.EntityFrameworkCore;
using System.Net;


public class RecipeService : IRecipeService
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IIngredientRepository _ingredientRepository;
    private readonly Random _random = new Random();

    public RecipeService(IRecipeRepository recipeRepository, IIngredientRepository ingredientRepository)
    {
        _recipeRepository = recipeRepository;
        _ingredientRepository = ingredientRepository;
    }

    public async Task<List<RecipeDto>> GetAllAsync()
    {
        List<Recipe> recipes = await _recipeRepository.GetAllAsync();

        return recipes.Select(r => new RecipeDto
        {
            Id = r.Id,
            Name = r.Name,
            Description = r.Description,
            ImageUrl = r.ImageUrl,
            Ingredients = r.Ingredients.Select(i => i.Name).ToList()
        }).ToList();
    }

    public async Task<RecipeDto> GetByIdAsync(int id)
    {
        var recipe = await _recipeRepository.GetByIdAsync(id);
        if (recipe == null) return null;

        return new RecipeDto
        {
            Id = recipe.Id,
            Name = recipe.Name,
            Description = recipe.Description,
            ImageUrl = recipe.ImageUrl,
            Ingredients = recipe.Ingredients.Select(i => i.Name).ToList()
        };
    }

    public async Task<List<Recipe>> GetByFilterAsync(string name, string ingredient)
    {
        return await _recipeRepository.GetByFilterAsync(name, ingredient);
    }

    public async Task<Recipe> AddAsync(Recipe recipe)
    {
        //nehezitett valtozat
        var processedIngredients = new List<Ingredient>();
        foreach (var i in recipe.Ingredients)
        {
            var existingIngredient = await _ingredientRepository.GetByNameAsync(i.Name);
            if (existingIngredient != null)
            { //processedIngredients instead-> recipe.Ing 
                processedIngredients.Add(existingIngredient);
            }
            else
            {
                var newIngredient = new Ingredient { Name = i.Name };
                await _ingredientRepository.AddAsync(newIngredient);
                processedIngredients.Add(newIngredient);
            }
        }
        recipe.Ingredients = processedIngredients;
        await _recipeRepository.AddAsync(recipe);
        return recipe;
    }

    public async Task<Recipe> UpdateAsync(int id, Recipe recipe)
    {
        var existingRecipe = await _recipeRepository.GetByIdAsync(id);
        if (existingRecipe == null)
            throw new KeyNotFoundException("Recipe not found.");

        existingRecipe.Name = recipe.Name;
        existingRecipe.Description = recipe.Description;
        existingRecipe.ImageUrl = recipe.ImageUrl;

        if (recipe.Ingredients != null && recipe.Ingredients.Any())
        {
            // csak uj hozzavalokat adhatunk hozza, a mar meglevo ne legyen modosithato
            foreach (var ingredient in recipe.Ingredients)
            {
                var existingIngredient = await _ingredientRepository.GetByNameAsync(ingredient.Name);
                if (existingIngredient != null)
                {
                    // csak akkor modositjuk ha meg nincs benne
                    if (!existingRecipe.Ingredients.Any(i => i.Name == existingIngredient.Name))
                    {
                        existingRecipe.Ingredients.Add(existingIngredient);
                    }
                }
                else
                {
                    var newIngredient = new Ingredient { Name = ingredient.Name };
                    await _ingredientRepository.AddAsync(newIngredient);//concat
                    existingRecipe.Ingredients.Add(newIngredient);
                }
            }
        }

        await _recipeRepository.UpdateAsync(existingRecipe);
        return existingRecipe;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _recipeRepository.DeleteAsync(id);
    }

    public async Task<Recipe?> GetRandomRecipeAsync(List<string> availableIngredients)
    {
        var recipes = await _recipeRepository.GetAllAsync();

        var matchingRecipes = recipes.Where(r => r.Ingredients.Any(i => availableIngredients.Contains(i.Name))).ToList();

        if (matchingRecipes.Any()) return matchingRecipes[_random.Next(matchingRecipes.Count)];
        else if (recipes.Any()) return recipes[_random.Next(recipes.Count)];
        else return null;

    }

}