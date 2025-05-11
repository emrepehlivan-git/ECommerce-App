using ECommerce.Application.Interfaces;
using ECommerce.Persistence.Contexts;
using ECommerce.Persistence;
using static OpenIddict.Abstractions.OpenIddictConstants;
using ECommerce.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using ECommerce.AuthServer;
using ECommerce.AuthServer.Services;
using static OpenIddict.Server.OpenIddictServerEvents;
using OpenIddict.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = "/Account/Login");

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
            Scopes.Address,
            Scopes.Email,
            Scopes.Phone,
            Scopes.Profile,
            Scopes.Roles);

        options.AllowAuthorizationCodeFlow()
               .RequireProofKeyForCodeExchange();

        options.AddDevelopmentEncryptionCertificate()
               .AddDevelopmentSigningCertificate();

        options.UseAspNetCore()
               .EnableAuthorizationEndpointPassthrough()
               .EnableTokenEndpointPassthrough()
               .EnableStatusCodePagesIntegration()
               .EnableUserInfoEndpointPassthrough()
               .EnableEndSessionEndpointPassthrough();

        options.IgnoreGrantTypePermissions();

        options.AddEventHandler<ProcessSignInContext>(builder =>
            builder.UseScopedHandler<AddClaimsToTokenHandler>()
            .SetType(OpenIddictServerHandlerType.Custom));
    })
    .AddValidation(options =>
    {
        options.UseLocalServer();
        options.UseAspNetCore();
    });

builder.Services.AddHostedService<Worker>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowAllOrigins");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();