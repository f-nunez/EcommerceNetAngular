using Microsoft.Extensions.Logging;

namespace Fnunez.Ena.Infrasctructure.Data;

public class StoreDbContextSeed
{
    public static async Task SeedAsync(StoreDbContext dbContext, ILoggerFactory loggerFactory)
    {
        try
        {
            //TODO: validate demo entities
            await Task.Delay(100);
        }
        catch (Exception e)
        {
            ILogger<StoreDbContext> logger = loggerFactory.CreateLogger<StoreDbContext>();
            logger.LogError(e.Message);
        }
    }
}