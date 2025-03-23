using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace ECommerce.SharedKernel;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddServicesRegistration(this IServiceCollection services, Assembly[] assemblies)
    {
        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo<ITransientDependency>())
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsSelf()
                .WithTransientLifetime()
            .AddClasses(classes => classes.AssignableTo<IScopedDependency>())
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsSelf()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo<ISingletonDependency>())
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsSelf()
                .WithSingletonLifetime()
        );

        services.AddTransient<ILazyServiceProvider, LazyServiceProvider>();

        return services;
    }
}

