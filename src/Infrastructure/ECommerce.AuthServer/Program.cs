using ECommerce.Persistence.Contexts;
using ECommerce.Persistence;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using ECommerce.SharedKernel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();
builder.Services.AddServicesRegistration([typeof(Program).Assembly]);

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddScoped<IIdentityService, IdentityService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, cookieOptions =>
{
    cookieOptions.LoginPath = "/Account/Login";
    cookieOptions.LogoutPath = "/Account/Logout";
});

builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
               .UseDbContext<ApplicationDbContext>();
    })
    .AddServer(options =>
    {
        options.SetAuthorizationEndpointUris("/connect/authorize")
               .SetTokenEndpointUris("/connect/token")
               .SetUserInfoEndpointUris("/connect/userinfo")
               .SetEndSessionEndpointUris("/connect/logout");

        options.RegisterScopes(
            Scopes.Email,
            Scopes.Profile,
            Scopes.Roles,
            Scopes.OpenId,
            "api");

        options.AllowAuthorizationCodeFlow()
               .AllowRefreshTokenFlow()
               .RequireProofKeyForCodeExchange();

        options.AddDevelopmentEncryptionCertificate()
               .AddDevelopmentSigningCertificate();

        options.UseAspNetCore()
               .EnableAuthorizationEndpointPassthrough()
               .EnableTokenEndpointPassthrough()
               .EnableStatusCodePagesIntegration()
               .EnableUserInfoEndpointPassthrough()
               .EnableEndSessionEndpointPassthrough();
    })
    .AddValidation(options =>
    {
        options.UseLocalServer();
        options.UseAspNetCore();
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
    var client = await manager.FindByClientIdAsync("nextjs-client");
    if (client is not null)
    {
        await manager.DeleteAsync(client);
    }
    await manager.CreateAsync(new OpenIddictApplicationDescriptor
    {
        ClientId = "nextjs-client",
        DisplayName = "Next.js Client",
        ConsentType = ConsentTypes.Explicit,
        RedirectUris =
        {
            new Uri("https://oauth.pstmn.io/v1/callback"),
            new Uri("http://localhost:3000/api/auth/callback/openiddict")
        },
        PostLogoutRedirectUris = { new Uri("http://localhost:3000/") },
        Permissions =
        {
            Permissions.Endpoints.Authorization,
            Permissions.Endpoints.EndSession,
            Permissions.Endpoints.Token,
            Permissions.GrantTypes.AuthorizationCode,
            Permissions.ResponseTypes.Code,
            Permissions.Scopes.Email,
            Permissions.Scopes.Profile,
            Permissions.Scopes.Roles,
            Permissions.Prefixes.Scope + "api"
        },
        Requirements =
        {
            Requirements.Features.ProofKeyForCodeExchange
        }
    });
}

app.Run();