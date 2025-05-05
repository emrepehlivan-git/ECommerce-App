using ECommerce.Application.Common.Interfaces;
using ECommerce.Infrastructure.Services;
using ECommerce.Persistence.Services;
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

        services.AddInfraServices();

        return services;
    }

    private static void AddInfraServices(this IServiceCollection services)
    {
        services.AddSingleton<ILocalizationService, LocalizationService>();
        services.AddSingleton<ICacheService, CacheService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}