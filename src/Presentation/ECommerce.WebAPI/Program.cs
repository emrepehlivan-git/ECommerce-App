using ECommerce.WebAPI;

ECommerce.Application.Common.Logging.ILogger logger = null!;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddPresentation(builder.Configuration);

    var app = builder.Build();

    await app.ApplyMigrations();

    logger = app.Services.GetRequiredService<ECommerce.Application.Common.Logging.ILogger>();

    app.UsePresentation(app.Environment);

    app.Run();
}
catch (Exception ex)
{
    logger.LogCritical("Application terminated unexpectedly: {Message}", ex.Message);
}