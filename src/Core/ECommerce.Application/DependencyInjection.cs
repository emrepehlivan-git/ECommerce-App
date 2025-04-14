using ECommerce.Application.Behaviors;
using ECommerce.Application.Common.Mappings;
using ECommerce.Application.Features.Categories.Commands;
using ECommerce.Application.Features.Categories;
using ECommerce.Application.Common.Helpers;
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
        {
            config.RegisterServicesFromAssembly(assembly);
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(CacheBehavior<,>));
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(TransactionalRequestBehavior<,>));
        });

        services.AddMapsterConfiguration();

        services.AddScoped<LocalizationHelper>();

        services.AddSingleton<ILazyServiceProvider, LazyServiceProvider>();

        services.AddScoped<CategoryBusinessRules>();

        return services;
    }
}