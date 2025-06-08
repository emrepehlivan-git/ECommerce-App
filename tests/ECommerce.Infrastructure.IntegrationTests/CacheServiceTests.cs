namespace ECommerce.Infrastructure.IntegrationTests;

public class CacheServiceTests
{
    private readonly IDistributedCache _cache;
    private readonly CacheService _cacheService;

    public CacheServiceTests()
    {
        _cache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
        _cacheService = new CacheService(_cache);
    }

    [Fact]
    public async Task SetAsync_ShouldStoreAndRetrieveValue()
    {
        var key = "test-key";
        var value = new TestRecord("value");

        await _cacheService.SetAsync(key, value, TimeSpan.FromMinutes(1));
        var result = await _cacheService.GetAsync<TestRecord>(key);

        result.Should().NotBeNull();
        result!.Value.Should().Be(value.Value);
    }

    [Fact]
    public async Task RemoveAsync_ShouldRemoveItemFromCache()
    {
        var key = "remove-key";
        await _cache.SetStringAsync(key, "data");

        await _cacheService.RemoveAsync(key);
        var result = await _cacheService.GetAsync<string>(key);

        result.Should().BeNull();
    }

    private record TestRecord(string Value);
}
