using System.Text.Json;
using Fnunez.Ena.Core.Interfaces;
using StackExchange.Redis;

namespace Fnunez.Ena.Infrastructure.Services;

public class ResponseCacheService : IResponseCacheService
{
    private readonly IDatabase _redisDb;

    public ResponseCacheService(IConnectionMultiplexer redisConnection)
    {
        _redisDb = redisConnection.GetDatabase();
    }

    public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
    {
        if (response == null)
            return;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        string serializedResponse = JsonSerializer.Serialize(response, options);

        await _redisDb.StringSetAsync(cacheKey, serializedResponse);
    }

    public async Task<string> GetCachedResponseAsync(string cacheKey)
    {
        RedisValue cachedResponse = await _redisDb.StringGetAsync(cacheKey);

        if (cachedResponse.IsNullOrEmpty)
            return null;

        return cachedResponse;
    }
}