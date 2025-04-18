using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Contexts;
using ECommerce.Persistence.Interceptors;
using ECommerce.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureDbContext(services, configuration);
        ConfigureIdentity(services);
        AddRepositories(services);

        return services;
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
    }

    private static void ConfigureDbContext(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
             {
                 var currentUserService = services.BuildServiceProvider().GetRequiredService<ICurrentUserService>();

                 options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                 options.UseSnakeCaseNamingConvention();
                 options.UseOpenIddict();
                 options.AddInterceptors(new AuditEntityInterceptor(currentUserService));
             });
    }

    private static void ConfigureIdentity(IServiceCollection services)
    {
        services.AddIdentity<User, Role>(options =>
         {
             options.Password.RequiredLength = 6;
             options.Password.RequireDigit = false;
             options.Password.RequireLowercase = false;
             options.Password.RequireUppercase = false;
             options.Password.RequireNonAlphanumeric = false;
             options.User.RequireUniqueEmail = true;
         })
         .AddEntityFrameworkStores<ApplicationDbContext>()
         .AddDefaultTokenProviders();
    }
}