using System.Text.Json;
using ECommerce.Application.Common.Interfaces;
using ECommerce.SharedKernel;
using Microsoft.Extensions.Caching.Distributed;
namespace ECommerce.Infrastructure.Services;

public sealed class CacheService : ICacheService, IScopedDependency
{
    private readonly IDistributedCache _cache;

    public CacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var cachedValue = await _cache.GetStringAsync(key);
        return cachedValue is null ? default : JsonSerializer.Deserialize<T>(cachedValue);
    }

    public async Task RemoveAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
    {
        var serializedValue = JsonSerializer.Serialize(value);
        await _cache.SetStringAsync(key, serializedValue, new DistributedCacheEntryOptions { AbsoluteExpiration = DateTimeOffset.Now.Add(expiration) });
    }
}
