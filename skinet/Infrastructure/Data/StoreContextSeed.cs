using System.ComponentModel;
using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            const string baseDirectory = "../Infrastructure/Data/SeedData/";

            if (!context.ProductBrands!.Any())
            {
                var brandsData = await File.ReadAllTextAsync(baseDirectory + "brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData) ?? new List<ProductBrand>();
                context.ProductBrands!.AddRange(brands);
            }

            if (!context.ProductTypes!.Any())
            {
                var typesData = await File.ReadAllTextAsync(baseDirectory + "types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData) ?? new List<ProductType>();
                context.AddRange(types);
            }

            if (!context.Products!.Any())
            {
                var productsData = await File.ReadAllTextAsync(baseDirectory + "products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData) ?? new List<Product>();
                context.AddRange(products);
            }

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }
        }
    }
}