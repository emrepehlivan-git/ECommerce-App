using System.Globalization;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using ECommerce.Application;
using ECommerce.Application.Constants;
using ECommerce.Application.Interfaces;
using ECommerce.Infrastructure;
using ECommerce.Persistence;
using ECommerce.Persistence.Contexts;
using ECommerce.SharedKernel;
using ECommerce.WebAPI.Authorization;
using ECommerce.WebAPI.Middlewares;
using ECommerce.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Validation.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace ECommerce.WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureLocalization(services);

        services.AddApplication()
            .AddInfrastructure(configuration)
            .AddPersistence(configuration);

        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddTransient<ILazyServiceProvider, LazyServiceProvider>();

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddHttpContextAccessor();
        services.AddProblemDetails();

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        });

        ConfigureOpenIddict(services, configuration);

        services.AddAuthorization();
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins", builder =>
            {
                builder.WithOrigins("http://localhost:3000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });

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

        app.UseCors("AllowAllOrigins");

        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        return app;
    }

    public static void AddAuthorization(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddAuthorization(options =>
        {
            // Products
            options.AddPolicy(PermissionConstants.Products.View, policy =>
                policy.AddRequirements(new PermissionRequirement(PermissionConstants.Products.View)));
            options.AddPolicy(PermissionConstants.Products.Create, policy =>
                policy.AddRequirements(new PermissionRequirement(PermissionConstants.Products.Create)));
            options.AddPolicy(PermissionConstants.Products.Update, policy =>
                policy.AddRequirements(new PermissionRequirement(PermissionConstants.Products.Update)));
            options.AddPolicy(PermissionConstants.Products.Delete, policy =>
                policy.AddRequirements(new PermissionRequirement(PermissionConstants.Products.Delete)));
            options.AddPolicy(PermissionConstants.Products.Manage, policy =>
                policy.AddRequirements(new PermissionRequirement(PermissionConstants.Products.Manage)));

            // Orders
            options.AddPolicy(PermissionConstants.Orders.View, policy =>
                policy.AddRequirements(new PermissionRequirement(PermissionConstants.Orders.View)));
            options.AddPolicy(PermissionConstants.Orders.Create, policy =>
                policy.AddRequirements(new PermissionRequirement(PermissionConstants.Orders.Create)));
            options.AddPolicy(PermissionConstants.Orders.Update, policy =>
                policy.AddRequirements(new PermissionRequirement(PermissionConstants.Orders.Update)));
            options.AddPolicy(PermissionConstants.Orders.Delete, policy =>
                policy.AddRequirements(new PermissionRequirement(PermissionConstants.Orders.Delete)));
            options.AddPolicy(PermissionConstants.Orders.Manage, policy =>
                policy.AddRequirements(new PermissionRequirement(PermissionConstants.Orders.Manage)));

            // Categories
            options.AddPolicy(PermissionConstants.Categories.View, policy =>
                policy.AddRequirements(new PermissionRequirement(PermissionConstants.Categories.View)));
            options.AddPolicy(PermissionConstants.Categories.Create, policy =>
                policy.AddRequirements(new PermissionRequirement(PermissionConstants.Categories.Create)));
            options.AddPolicy(PermissionConstants.Categories.Update, policy =>
                policy.AddRequirements(new PermissionRequirement(PermissionConstants.Categories.Update)));
            options.AddPolicy(PermissionConstants.Categories.Delete, policy =>
                policy.AddRequirements(new PermissionRequirement(PermissionConstants.Categories.Delete)));
            options.AddPolicy(PermissionConstants.Categories.Manage, policy =>
                policy.AddRequirements(new PermissionRequirement(PermissionConstants.Categories.Manage)));

            // Users
            options.AddPolicy(PermissionConstants.Users.View, policy =>
                policy.AddRequirements(new PermissionRequirement(PermissionConstants.Users.View)));
            options.AddPolicy(PermissionConstants.Users.Create, policy =>
                policy.AddRequirements(new PermissionRequirement(PermissionConstants.Users.Create)));
            options.AddPolicy(PermissionConstants.Users.Update, policy =>
                policy.AddRequirements(new PermissionRequirement(PermissionConstants.Users.Update)));
            options.AddPolicy(PermissionConstants.Users.Delete, policy =>
                policy.AddRequirements(new PermissionRequirement(PermissionConstants.Users.Delete)));
            options.AddPolicy(PermissionConstants.Users.Manage, policy =>
                policy.AddRequirements(new PermissionRequirement(PermissionConstants.Users.Manage)));

            // Roles
            options.AddPolicy(PermissionConstants.Roles.View, policy =>
                policy.AddRequirements(new PermissionRequirement(PermissionConstants.Roles.View)));
            options.AddPolicy(PermissionConstants.Roles.Create, policy =>
                policy.AddRequirements(new PermissionRequirement(PermissionConstants.Roles.Create)));
            options.AddPolicy(PermissionConstants.Roles.Update, policy =>
                policy.AddRequirements(new PermissionRequirement(PermissionConstants.Roles.Update)));
            options.AddPolicy(PermissionConstants.Roles.Delete, policy =>
                policy.AddRequirements(new PermissionRequirement(PermissionConstants.Roles.Delete)));
            options.AddPolicy(PermissionConstants.Roles.Manage, policy =>
                policy.AddRequirements(new PermissionRequirement(PermissionConstants.Roles.Manage)));
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

    private static void ConfigureOpenIddict(IServiceCollection services, IConfiguration configuration)
    {
        X509Certificate2? caCert = null;
        if (File.Exists("/app/ca.crt"))
        {
            caCert = new X509Certificate2("/app/ca.crt");
        }

        services.AddOpenIddict()
            .AddValidation(options =>
            {
                options.SetIssuer(new Uri(configuration["Authentication:Authority"]!));
                options.AddAudiences(configuration["Authentication:Audience"]!);

                options.UseSystemNetHttp()
                .ConfigureHttpClientHandler(handler =>
                {
                    if (caCert is not null)
                    {
                        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                        {
                            if (errors == SslPolicyErrors.None)
                                return true;

                            var chainWithExtra = new X509Chain();
                            chainWithExtra.ChainPolicy.ExtraStore.Add(caCert);
                            chainWithExtra.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
                            chainWithExtra.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;

                            return chainWithExtra.Build(cert ?? throw new InvalidOperationException("Certificate is null"));
                        };
                    }
                });


                options.UseAspNetCore();

            });
    }

    private static void ConfigureLocalization(IServiceCollection services)
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
    }
}