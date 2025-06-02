using ECommerce.Application.Interfaces;
using ECommerce.Persistence.Contexts;
using ECommerce.Persistence;
using static OpenIddict.Abstractions.OpenIddictConstants;
using ECommerce.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using ECommerce.AuthServer;
using ECommerce.AuthServer.Services;
using ECommerce.Application.Services;
using ECommerce.Infrastructure;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

X509Certificate2? caCert = null;
if (File.Exists("/app/ca.crt"))
{
    caCert = new X509Certificate2("/app/ca.crt");
}

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
        options.SetIssuer(new Uri("https://ecommerce.authserver:8081/"));

        options.SetAuthorizationEndpointUris("/connect/authorize")
               .SetTokenEndpointUris("/connect/token")
               .SetUserInfoEndpointUris("/connect/userinfo")
               .SetIntrospectionEndpointUris("/connect/introspect")
               .SetEndSessionEndpointUris("/connect/logout");

        options.RegisterScopes(
            Scopes.Address,
            Scopes.Email,
            Scopes.Phone,
            Scopes.Profile,
            Scopes.Roles,
            "api");

        options.AllowAuthorizationCodeFlow()
                .AllowClientCredentialsFlow()
                .RequireProofKeyForCodeExchange();

        options.AddDevelopmentEncryptionCertificate()
               .AddDevelopmentSigningCertificate();

        options.DisableAccessTokenEncryption();

        options.UseAspNetCore()
               .EnableAuthorizationEndpointPassthrough()
               .EnableTokenEndpointPassthrough()
               .EnableStatusCodePagesIntegration()
               .EnableUserInfoEndpointPassthrough()
               .EnableEndSessionEndpointPassthrough();

        options.IgnoreGrantTypePermissions();

    })
    .AddValidation(options =>
    {
        options.UseLocalServer();
        options.UseAspNetCore();
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