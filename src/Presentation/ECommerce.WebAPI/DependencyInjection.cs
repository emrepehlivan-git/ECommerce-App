using System.Globalization;
using ECommerce.Application;
using ECommerce.Application.Constants;
using ECommerce.Application.Interfaces;
using ECommerce.Infrastructure;
using ECommerce.Persistence;
using ECommerce.Persistence.Contexts;
using ECommerce.SharedKernel;
using ECommerce.WebAPI.Extensions;
using ECommerce.WebAPI.Middlewares;
using ECommerce.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;

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

        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddAuthorization();

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

    public static void AddAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            // Products
            options.AddPolicy(PermissionConstants.Products.View, policy =>
                policy.RequirePermission(PermissionConstants.Products.View));
            options.AddPolicy(PermissionConstants.Products.Create, policy =>
                policy.RequirePermission(PermissionConstants.Products.Create));
            options.AddPolicy(PermissionConstants.Products.Update, policy =>
                policy.RequirePermission(PermissionConstants.Products.Update));
            options.AddPolicy(PermissionConstants.Products.Delete, policy =>
                policy.RequirePermission(PermissionConstants.Products.Delete));
            options.AddPolicy(PermissionConstants.Products.Manage, policy =>
                policy.RequirePermission(PermissionConstants.Products.Manage));

            // Orders
            options.AddPolicy(PermissionConstants.Orders.View, policy =>
                policy.RequirePermission(PermissionConstants.Orders.View));
            options.AddPolicy(PermissionConstants.Orders.Create, policy =>
                policy.RequirePermission(PermissionConstants.Orders.Create));
            options.AddPolicy(PermissionConstants.Orders.Update, policy =>
                policy.RequirePermission(PermissionConstants.Orders.Update));
            options.AddPolicy(PermissionConstants.Orders.Delete, policy =>
                policy.RequirePermission(PermissionConstants.Orders.Delete));
            options.AddPolicy(PermissionConstants.Orders.Manage, policy =>
                policy.RequirePermission(PermissionConstants.Orders.Manage));

            // Categories
            options.AddPolicy(PermissionConstants.Categories.View, policy =>
                policy.RequirePermission(PermissionConstants.Categories.View));
            options.AddPolicy(PermissionConstants.Categories.Create, policy =>
                policy.RequirePermission(PermissionConstants.Categories.Create));
            options.AddPolicy(PermissionConstants.Categories.Update, policy =>
                policy.RequirePermission(PermissionConstants.Categories.Update));
            options.AddPolicy(PermissionConstants.Categories.Delete, policy =>
                policy.RequirePermission(PermissionConstants.Categories.Delete));
            options.AddPolicy(PermissionConstants.Categories.Manage, policy =>
                policy.RequirePermission(PermissionConstants.Categories.Manage));

            // Users
            options.AddPolicy(PermissionConstants.Users.View, policy =>
                policy.RequirePermission(PermissionConstants.Users.View));
            options.AddPolicy(PermissionConstants.Users.Create, policy =>
                policy.RequirePermission(PermissionConstants.Users.Create));
            options.AddPolicy(PermissionConstants.Users.Update, policy =>
                policy.RequirePermission(PermissionConstants.Users.Update));
            options.AddPolicy(PermissionConstants.Users.Delete, policy =>
                policy.RequirePermission(PermissionConstants.Users.Delete));
            options.AddPolicy(PermissionConstants.Users.Manage, policy =>
                policy.RequirePermission(PermissionConstants.Users.Manage));

            // Roles
            options.AddPolicy(PermissionConstants.Roles.View, policy =>
                policy.RequirePermission(PermissionConstants.Roles.View));
            options.AddPolicy(PermissionConstants.Roles.Create, policy =>
                policy.RequirePermission(PermissionConstants.Roles.Create));
            options.AddPolicy(PermissionConstants.Roles.Update, policy =>
                policy.RequirePermission(PermissionConstants.Roles.Update));
            options.AddPolicy(PermissionConstants.Roles.Delete, policy =>
                policy.RequirePermission(PermissionConstants.Roles.Delete));
            options.AddPolicy(PermissionConstants.Roles.Manage, policy =>
                policy.RequirePermission(PermissionConstants.Roles.Manage));
        });
    }

    public static async Task ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
            await dbContext.Database.MigrateAsync();
    }
}
