using Microsoft.EntityFrameworkCore;

namespace first
{
    public class RecipeContext : DbContext
    {
        public RecipeContext(DbContextOptions<RecipeContext> options) : base(options)
        {
        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ingredient>()
                .Property(i => i.Id)
                .ValueGeneratedOnAdd(); // automatikus id generalas

            modelBuilder.Entity<Recipe>()
                .Property(r => r.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.Ingredients)
                .WithMany(i => i.Recipes)
                .UsingEntity(j => j.ToTable("RecipeIngredients"));
        }
    }
}