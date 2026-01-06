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
                return;

            // ========== ROOT CATEGORIES ==========
            var men = Category.Create(
                "Men",
                "Men's clothing",
                null,
                0,
                "men-icon",
                "#1E88E5"
            ).Value;
            _dbContext.Categories.Add(men);

            var women = Category.Create(
                "Women",
                "Women's clothing",
                null,
                1,
                "women-icon",
                "#E91E63"
            ).Value;
            _dbContext.Categories.Add(women);

            var kids = Category.Create(
                "Kids",
                "Kids clothing",
                null,
                2,
                "kids-icon",
                "#FF9800"
            ).Value;
            _dbContext.Categories.Add(kids);

            var accessories = Category.Create(
                "Accessories",
                "Fashion accessories",
                null,
                3,
                "accessories-icon",
                "#6D4C41"
            ).Value;
            _dbContext.Categories.Add(accessories);

            // ========== MEN ==========
            var menTops = Category.Create("Tops", "Upper wear for men", men, 0).Value;
            var menBottoms = Category.Create("Bottoms", "Lower wear for men", men, 1).Value;
            var menOuterwear = Category.Create("Outerwear", "Jackets & coats", men, 2).Value;

            _dbContext.Categories.AddRange(menTops, menBottoms, menOuterwear);

            _dbContext.Categories.AddRange(
                Category.Create("T-Shirts", "Casual t-shirts", menTops, 0).Value,
                Category.Create("Shirts", "Formal & casual shirts", menTops, 1).Value,
                Category.Create("Polo Shirts", "Collared casual wear", menTops, 2).Value,

                Category.Create("Jeans", "Denim wear", menBottoms, 0).Value,
                Category.Create("Trousers", "Formal trousers", menBottoms, 1).Value,
                Category.Create("Shorts", "Casual shorts", menBottoms, 2).Value
            );

            // ========== WOMEN ==========
            var womenTops = Category.Create("Tops", "Upper wear for women", women, 0).Value;
            var womenBottoms = Category.Create("Bottoms", "Lower wear for women", women, 1).Value;
            var dresses = Category.Create("Dresses", "All types of dresses", women, 2).Value;

            _dbContext.Categories.AddRange(womenTops, womenBottoms, dresses);

            _dbContext.Categories.AddRange(
                Category.Create("Blouses", "Formal & casual blouses", womenTops, 0).Value,
                Category.Create("T-Shirts", "Casual t-shirts", womenTops, 1).Value,

                Category.Create("Casual Dresses", "Everyday dresses", dresses, 0).Value,
                Category.Create("Party Dresses", "Evening & party wear", dresses, 1).Value
            );

            // ========== KIDS ==========
            var boys = Category.Create("Boys", "Clothing for boys", kids, 0).Value;
            var girls = Category.Create("Girls", "Clothing for girls", kids, 1).Value;

            _dbContext.Categories.AddRange(boys, girls);

            _dbContext.Categories.AddRange(
                Category.Create("Tops", "T-shirts & shirts", boys, 0).Value,
                Category.Create("Bottoms", "Jeans & shorts", boys, 1).Value,

                Category.Create("Tops", "T-shirts & blouses", girls, 0).Value,
                Category.Create("Bottoms", "Skirts & leggings", girls, 1).Value
            );

            // ========== ACCESSORIES ==========
            var footwear = Category.Create("Footwear", "Shoes & sandals", accessories, 0).Value;
            var bags = Category.Create("Bags", "Handbags & backpacks", accessories, 1).Value;

            _dbContext.Categories.AddRange(footwear, bags);

            _dbContext.Categories.AddRange(
                Category.Create("Sneakers", "Casual shoes", footwear, 0).Value,
                Category.Create("Sandals", "Open footwear", footwear, 1).Value
            );

            await _dbContext.SaveChangesAsync();
        }

    }
}
