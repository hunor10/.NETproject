public class RecipeDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public List<string> Ingredients { get; set; } = new List<string>();
}