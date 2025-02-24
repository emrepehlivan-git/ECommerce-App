using ECommerce.Application.Behaviors;
using ECommerce.Application.Features.Categories.Commands;
using ECommerce.SharedKernel;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddValidatorsFromAssemblyContaining<CreateCategoryCommandValidator>(includeInternalTypes: true);

        services.AddMediatR(config =>
            config.RegisterServicesFromAssembly(assembly)
            .AddOpenBehavior(typeof(ValidationBehavior<,>))
            .AddOpenBehavior(typeof(CacheBehavior<,>))
            .AddOpenBehavior(typeof(TransactionalRequest<,>)));

        services.AddServicesRegistration([assembly]);

        return services;
    }
}