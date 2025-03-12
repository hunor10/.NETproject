using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;

[ApiController]
[Route("api/recipes")]
public class RecipeController : ControllerBase
{
    private readonly IRecipeService _recipeService;

    public RecipeController(IRecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RecipeDto>>> GetRecipes()
    {
        var recipes = await _recipeService.GetAllAsync();
       
        return StatusCode((int)HttpStatusCode.OK,recipes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Recipe>> GetRecipe(int id)
    {
        var recipe = await _recipeService.GetByIdAsync(id);
        if (recipe==null) return StatusCode((int)HttpStatusCode.NotFound, "Recipe aint found");
       
        return StatusCode((int)HttpStatusCode.OK, recipe);
    }

    [HttpPost]
    public async Task<ActionResult<Recipe>> CreateRecipe([FromBody] Recipe recipe)
    {
        if (recipe==null) return StatusCode((int)HttpStatusCode.BadRequest, "Invalid request");
  
        var createdRecipe=await _recipeService.AddAsync(recipe);
        
        return CreatedAtAction(nameof(GetRecipe), new { id=createdRecipe.Id }, createdRecipe);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Recipe>> UpdateRecipe(int id, [FromBody] Recipe recipe)
    {
        if (recipe == null) return StatusCode((int)HttpStatusCode.BadRequest, "ID invalid");

        var updatedRecipe = await _recipeService.UpdateAsync(id, recipe);
        if (updatedRecipe == null) return StatusCode((int)HttpStatusCode.NotFound, "Recipe aint foundd");

        return StatusCode((int)HttpStatusCode.OK, updatedRecipe);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteRecipe(int id)
    {
        var result = await _recipeService.DeleteAsync(id);
        if (!result) return StatusCode((int)HttpStatusCode.NotFound, "Recipe aint found");
      
        return StatusCode((int)HttpStatusCode.NoContent);
    }

    [HttpGet("random")]
    public async Task<IActionResult> GetRandomRecipe([FromQuery] List<string> ingredients)
    {
        //kerunk egy random hozzavalot
        var recipe = await _recipeService.GetRandomRecipeAsync(ingredients);
        //ha a hozzavalo letezik akkor visszateritjuk , kulonben not found statusz kod a response ba
        return recipe != null ? Ok(recipe) : NotFound("No recipes found.");
    }
}
