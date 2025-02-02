using ECommerce.Application.Common.Helpers;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
        });
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddSingleton<ILocalizationService, LocalizationService>();
        services.AddSingleton<L>();

        return services;
    }
}