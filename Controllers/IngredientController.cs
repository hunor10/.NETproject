using Microsoft.AspNetCore.Mvc;
using System.Net;

[ApiController]
[Route("api/ingredients")]
public class IngredientsController : ControllerBase{
    private readonly IIngredientService _ingredientService;

    public IngredientsController(IIngredientService ingredientService){
        _ingredientService=ingredientService;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Ingredient>>> GetIngredients(){
        var ingredients=await _ingredientService.GetAllAsync();
       
        return StatusCode((int)HttpStatusCode.OK, ingredients); //explicit statusz kodok extra pontert
    }

    [HttpGet("{id}")] //query parameter 1. eloadas vegen
    public async Task<ActionResult<Ingredient>> GetIngredient(int id){
        var ingredient=await _ingredientService.GetByIdAsync(id); 
        if(ingredient==null) return StatusCode((int)HttpStatusCode.NotFound, "Ingredient aint found");
       
        return StatusCode((int)HttpStatusCode.OK, ingredient);
    }

    [HttpPost]
public async Task<ActionResult<Ingredient>> CreateIngredient([FromBody] IngredientDto ingredientDto)
{
    if (ingredientDto == null) return BadRequest();

    var ingredient = new Ingredient
    {
        Name = ingredientDto.Name
    };

    var createdIngredient = await _ingredientService.AddAsync(ingredient);
   
    return CreatedAtAction(nameof(GetIngredient), new { id = createdIngredient.Id }, createdIngredient);
}

     
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIngredient(int id, [FromBody] IngredientDto ingredientDto)
    {
        if (ingredientDto == null) return StatusCode((int)HttpStatusCode.BadRequest, "Ingredient data required");
        var existingIngredient = await _ingredientService.GetByIdAsync(id);
        if (existingIngredient == null) return StatusCode((int)HttpStatusCode.NotFound, $"Ingredient wasnt found");

        existingIngredient.Name = ingredientDto.Name; 
        var updatedIngredient = await _ingredientService.UpdateAsync(id, existingIngredient);
        
        return StatusCode((int)HttpStatusCode.OK, updatedIngredient); 
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteIngredient(int id){
        var result=await _ingredientService.DeleteAsync(id);
        if(!result) return StatusCode((int)HttpStatusCode.NotFound, $"Ingredient id was not found");

        return StatusCode((int)HttpStatusCode.NoContent);
    }

}