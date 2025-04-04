using ECommerce.Application.Common.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Contexts;
using ECommerce.Persistence.Interceptors;
using ECommerce.SharedKernel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            {
                var currentUserService = services.BuildServiceProvider().GetRequiredService<ICurrentUserService>();

                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                options.UseSnakeCaseNamingConvention();
                options.UseOpenIddict();
                options.AddInterceptors(new AuditEntityInterceptor(currentUserService));
            });

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

        services.AddServicesRegistration([typeof(DependencyInjection).Assembly]);

        return services;
    }
}