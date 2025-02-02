using ECommerce.Persistence.Contexts;
using ECommerce.Persistence;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddPersistence(builder.Configuration);

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
               .SetUserInfoEndpointUris("/connect/userinfo");

        options.AllowAuthorizationCodeFlow()
               .AllowRefreshTokenFlow()
               .RequireProofKeyForCodeExchange();

        options.AddDevelopmentEncryptionCertificate()
               .AddDevelopmentSigningCertificate();

        options.RegisterScopes(
            Scopes.Email,
            Scopes.Profile,
            Scopes.Roles,
            "api"
        );

        options.UseAspNetCore()
               .EnableAuthorizationEndpointPassthrough()
               .EnableTokenEndpointPassthrough()
               .EnableStatusCodePagesIntegration()
               .EnableUserInfoEndpointPassthrough();
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

using (var scope = app.Services.CreateScope())
{
    var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

    if (await manager.FindByClientIdAsync("nextjs-client") == null)
    {
        await manager.CreateAsync(new OpenIddictApplicationDescriptor
        {
            ClientId = "nextjs-client",
            DisplayName = "Next.js Client",
            RedirectUris = { new Uri("http://localhost:3000/api/auth/callback/openiddict") },
            PostLogoutRedirectUris = { new Uri("http://localhost:3000/") },
            Permissions =
            {
                Permissions.Endpoints.Authorization,
                Permissions.Endpoints.Token,
                GrantTypes.AuthorizationCode,
                GrantTypes.RefreshToken,
                ResponseTypes.Code,
                Scopes.Email,
                Scopes.Profile,
                Scopes.Roles,
                "api"
            },
            Requirements =
            {
                Requirements.Features.ProofKeyForCodeExchange
            },
            ConsentType = ConsentTypes.Explicit,
        });
    }
}

app.Run();