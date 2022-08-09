using System.Text.Json;
using Fnunez.Ena.Core.Entities;
using Microsoft.Extensions.Logging;

namespace Fnunez.Ena.Infrasctructure.Data;

public class StoreDbContextSeed
{
    public static async Task SeedAsync(StoreDbContext dbContext, ILoggerFactory loggerFactory)
    {
        try
        {
            if (!dbContext.ProductBrands.Any())
            {
                string brandsData = File.ReadAllText("../Fnunez.Ena.Infrastructure/Data/SeedData/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                foreach (var item in brands)
                    dbContext.ProductBrands.Add(item);

                await dbContext.SaveChangesAsync();
            }

            if (!dbContext.ProductTypes.Any())
            {
                string typesData = File.ReadAllText("../Fnunez.Ena.Infrastructure/Data/SeedData/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                foreach (var item in types)
                    dbContext.ProductTypes.Add(item);

                await dbContext.SaveChangesAsync();
            }

            if (!dbContext.Products.Any())
            {
                string productsData = File.ReadAllText("../Fnunez.Ena.Infrastructure/Data/SeedData/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                foreach (var item in products)
                    dbContext.Products.Add(item);

                await dbContext.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            ILogger<StoreDbContext> logger = loggerFactory.CreateLogger<StoreDbContext>();
            logger.LogError(e.Message);
        }
    }
}