using System.Globalization;
using ECommerce.Application;
using ECommerce.Infrastructure;
using ECommerce.Persistence;
using ECommerce.SharedKernel;
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
        services.AddServicesRegistration([typeof(DependencyInjection).Assembly]);

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddInfrastructure(configuration);
        services.AddApplication();
        services.AddPersistence(configuration);
        services.AddHttpContextAccessor();

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

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}
