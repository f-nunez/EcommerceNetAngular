using System.Text.Json;
using Fnunez.Ena.Core.Entities;
using Fnunez.Ena.Core.Interfaces;
using StackExchange.Redis;

namespace Fnunez.Ena.Infrastructure.Data;

public class BasketRepository : IBasketRepository
{
    private readonly IDatabase _database;

    public BasketRepository(IConnectionMultiplexer redisConnection)
    {
        _database = redisConnection.GetDatabase();
    }

    public async Task<bool> DeleteBasketAsync(string basketId)
    {
        return await _database.KeyDeleteAsync(basketId);
    }

    public async Task<CustomerBasket> GetBasketAsync(string basketId)
    {
        RedisValue data = await _database.StringGetAsync(basketId);

        return data.IsNullOrEmpty ? null
            : JsonSerializer.Deserialize<CustomerBasket>(data);
    }

    public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
    {
        bool isCreated = await _database.StringSetAsync(
            basket.Id,
            JsonSerializer.Serialize(basket),
            TimeSpan.FromDays(30)
        );

        if (isCreated)
            return await GetBasketAsync(basket.Id);

        return null;
    }
}