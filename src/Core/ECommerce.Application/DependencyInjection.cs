using ECommerce.Application.Behaviors;
using ECommerce.Application.Common.Helpers;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(CacheBehavior<,>));
        });

        services.AddValidatorsFromAssembly(assembly);

        services.AddSingleton<L>();

        return services;
    }
}