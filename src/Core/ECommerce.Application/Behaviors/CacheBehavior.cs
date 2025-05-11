using ECommerce.Application.Interfaces;
using MediatR;

namespace ECommerce.Application.Behaviors;

public sealed class CacheBehavior<TRequest, TResponse>(ICacheService cacheService) : IPipelineBehavior<TRequest, TResponse>
where TRequest : ICacheableRequest
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var cacheKey = request.CacheKey;
        var cachedValue = await cacheService.GetAsync<TResponse>(cacheKey);

        if (cachedValue is not null)
            return cachedValue;

        var result = await next();

        await cacheService.SetAsync(cacheKey, result, request.CacheDuration);

        return result;
    }
}
