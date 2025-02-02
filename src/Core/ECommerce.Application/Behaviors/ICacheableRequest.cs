namespace ECommerce.Application.Behaviors;

public interface ICacheableRequest
{
    string CacheKey { get; }
    TimeSpan CacheDuration { get; }
}
