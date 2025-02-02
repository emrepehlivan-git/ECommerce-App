using ECommerce.Application.Common.Interfaces;
using MediatR;

namespace ECommerce.Application.Behaviors;

public sealed class CacheBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : ICacheableRequest
{
    private readonly ICacheService _cacheService;

    public CacheBehavior(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var cacheKey = request.CacheKey;
        var cachedValue = await _cacheService.GetAsync<TResponse>(cacheKey);

        if (cachedValue is not null)
            return cachedValue;

        var result = await next();

        await _cacheService.SetAsync(cacheKey, result, request.CacheDuration);

        return result;
    }
}
