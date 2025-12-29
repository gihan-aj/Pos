using Pos.Web.Features.Catalog.Entities;

namespace Pos.Web.Infrastructure.Persistence.Seed
{
    public class DataSeeder
    {
        private readonly AppDbContext _dbContext;

        public DataSeeder(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SeedAsync()
        {
            if (_dbContext.Categories.Any())
            {
                return; // Already seeded
            }

            // 1. Root: Beverages (Active, Order 1)
            var beveragesResult = Category.Create("Beverages", "Drinks and liquids", null, 1, "beverage-icon", "#FF0000");
            var beverages = beveragesResult.Value;
            _dbContext.Categories.Add(beverages);

            // 1.1 Child: Soft Drinks (Active)
            var softDrinksResult = Category.Create("Soft Drinks", "Carbonated beverages", beverages, 1, null, null);
            var softDrinks = softDrinksResult.Value;
            _dbContext.Categories.Add(softDrinks);

            // 1.1.1 Grandchild: Colas (Active)
            var colas = Category.Create("Colas", "Coke, Pepsi, etc.", softDrinks, 1, null, null).Value;
            _dbContext.Categories.Add(colas);

            // 1.1.2 Grandchild: Energy Drinks (Active)
            var energy = Category.Create("Energy Drinks", "Red Bull, Monster", softDrinks, 2, null, null).Value;
            _dbContext.Categories.Add(energy);

            // 1.2 Child: Alcohol (Inactive - Test filtering)
            var alcoholResult = Category.Create("Alcohol", "21+ Only", beverages, 2, null, null);
            var alcohol = alcoholResult.Value;
            alcohol.Deactivate(); // Deactivate immediately
            _dbContext.Categories.Add(alcohol);

            // 1.2.1 Grandchild: Beer (Inactive because parent is inactive)
            var beer = Category.Create("Beer", "Domestic and Imported", alcohol, 1, null, null).Value;
            beer.Deactivate(); // Domain logic in 'alcohol.Deactivate()' usually handles this, but during seed we might need manual calls depending on order
            _dbContext.Categories.Add(beer);

            // 2. Root: Fresh Produce (Active, Order 0 - Should appear first in sort)
            var produceResult = Category.Create("Fresh Produce", "Fruits and Veggies", null, 0, "leaf-icon", "#00FF00");
            var produce = produceResult.Value;
            _dbContext.Categories.Add(produce);

            // 2.1 Child: Fruits
            var fruitsResult = Category.Create("Fruits", "Fresh fruits", produce, 1, null, null);
            var fruits = fruitsResult.Value;
            _dbContext.Categories.Add(fruits);

            // 2.1.1 Grandchild: Tropical
            var tropical = Category.Create("Tropical", "Bananas, Pineapples", fruits, 1, null, null).Value;
            _dbContext.Categories.Add(tropical);

            // 2.1.2 Grandchild: Berries
            var berries = Category.Create("Berries", "Strawberries, Blueberries", fruits, 2, null, null).Value;
            _dbContext.Categories.Add(berries);

            // 2.2 Child: Vegetables
            var veg = Category.Create("Vegetables", "Roots and greens", produce, 2, null, null).Value;
            _dbContext.Categories.Add(veg);

            // 3. Root: Bakery (Active, Order 2)
            var bakery = Category.Create("Bakery", "Fresh bread daily", null, 2, "bread-icon", "#FFA500").Value;
            _dbContext.Categories.Add(bakery);

            await _dbContext.SaveChangesAsync();
        }
    }
}
