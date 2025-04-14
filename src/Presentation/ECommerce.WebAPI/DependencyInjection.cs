using System.Globalization;
using ECommerce.Application;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Infrastructure;
using ECommerce.Persistence;
using ECommerce.SharedKernel;
using ECommerce.WebAPI.Middlewares;
using ECommerce.WebAPI.Services;
using Microsoft.AspNetCore.Localization;

namespace ECommerce.WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        var supportedCultures = new[]
        {
            new CultureInfo("en-US"),
            new CultureInfo("tr-TR"),
        };

        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture("en-US");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.RequestCultureProviders =
            [
                new AcceptLanguageHeaderRequestCultureProvider()
            ];
        });
        services.AddApplication();
        services.AddInfrastructure(configuration);
        services.AddPersistence(configuration);

        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddTransient<ILazyServiceProvider, LazyServiceProvider>();

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddHttpContextAccessor();
        services.AddProblemDetails();

        return services;
    }

    public static WebApplication UsePresentation(this WebApplication app, IWebHostEnvironment environment)
    {

        app.UseRequestLocalization();

        if (environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}
